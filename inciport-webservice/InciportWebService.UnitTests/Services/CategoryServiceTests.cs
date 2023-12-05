using AutoFixture;
using FluentAssertions;
using InciportWebService.Application;
using InciportWebService.Data;
using InciportWebService.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace InciportWebService.UnitTests.Services {

  public class CategoryServiceTests : IDisposable {
    private DbContextOptions<ApplicationDbContext> _options;
    private ApplicationDbContext _dbContext;
    private Fixture _fixture;
    private MainCategory _mainCategory;
    private CategoriesService _categoryService;
    private const int MUNICIPALITYID = 1;

    public CategoryServiceTests() {
      _options = new DbContextOptionsBuilder<ApplicationDbContext>()
      .UseInMemoryDatabase(databaseName: "inciport")
      .Options;
      _dbContext = new ApplicationDbContext(_options);

      _categoryService = new CategoriesService(_dbContext);

      _fixture = new Fixture();
      _fixture.Register(() => new Category { IsArchived = false });
      Category subCategory1 = _fixture.Create<Category>();
      List<Category> subCategories = new List<Category> { subCategory1 };
      _fixture.Register(() => new MainCategory { IsArchived = false, SubCategories = subCategories });

      _mainCategory = _fixture.Create<MainCategory>();

      List<MainCategory> categories = new List<MainCategory> { _mainCategory };
      var municipality = new MunicipalityEntity { Id = MUNICIPALITYID, Name = "Test", MainCategories = categories };

      _dbContext.Add(municipality);
      _dbContext.SaveChanges();
    }

    public void Dispose() {
      _dbContext.Database.EnsureDeleted();
      _dbContext.Dispose();
    }

    [Fact]
    public async Task GetCategoriesAsync_WithNoArchived_ReturnsAllCategories() {
      //ARRANGE
      List<MainCategory> expected = new List<MainCategory> { expectedResultHelper(_mainCategory) };

      //ACT
      var actual = await _categoryService.GetCategoriesAsync(MUNICIPALITYID) as List<MainCategory>;

      //ASSERT
      actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetCategoriesAsync_WithArchived_ReturnsEmptyList() {
      //ARRANGE
      List<MainCategory> expected = new List<MainCategory>();
      _mainCategory.Archive();
      _dbContext.SaveChanges();

      //ACT
      var actual = await _categoryService.GetCategoriesAsync(MUNICIPALITYID) as List<MainCategory>;

      //ASSERT
      actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetCategoryAsync_WithNoArchived_ReturnsCorrectCategory() {
      //ARRANGE
      var expected = expectedResultHelper(_mainCategory);
      //ACT
      var actual = await _categoryService.GetCategoryAsync(MUNICIPALITYID, _mainCategory.Id) as MainCategory;
      //ASSERT
      actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetCategoryAsync_WithArchived_ReturnsNotFound() {
      //ARRANGE
      _mainCategory.Archive();
      _dbContext.SaveChanges();

      //ACT

      //ASSERT
      await Assert.ThrowsAsync<NotFoundException>(() => _categoryService.GetCategoryAsync(MUNICIPALITYID, _mainCategory.Id));
    }

    [Fact]
    public async Task CreateCategoryAsync_WithValidCategory_ReturnsCorrectResult() {
      //ARRANGE

      List<CreateCategoryDto> subCategories = _mainCategory.SubCategories.Select(c => new CreateCategoryDto { Title = c.Title }).ToList();

      CreateMainCategoryDto categoryToCreate = new CreateMainCategoryDto { Title = _mainCategory.Title, SubCategories = subCategories };
      MainCategory expected = categoryToCreate.ToModel();

      //ACT
      _ = await _categoryService.CreateCategoryAsync(MUNICIPALITYID, categoryToCreate) as MainCategory;
      var actualCategorySaved = _dbContext.Municipalities.Find(MUNICIPALITYID).MainCategories.First();
      expected.Id = actualCategorySaved.Id;
      expected.SubCategories[0].Id = actualCategorySaved.SubCategories[0].Id;

      //ASSERT
      actualCategorySaved.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task UpdateCategoryAsync_WithChangedTitle_ReturnsCorrectResult() {
      //ARRANGE

      CategoryDto input = CategoryDto.FromModel(_mainCategory);
      input.Title = "New title";
      MainCategory expected = expectedResultHelper(_mainCategory);
      expected.Title = input.Title;
      //ACT
      MainCategory actualUpdatedCategory = await _categoryService.UpdateCategoryAsync(MUNICIPALITYID, input) as MainCategory;

      expected.Id = actualUpdatedCategory.Id;
      expected.SubCategories[0].Id = actualUpdatedCategory.SubCategories[0].Id;

      //ASSERT
      actualUpdatedCategory.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task DeleteCategoryAsync_WithValidId_GetsArchivedCorrectly() {
      //ARRANGE
      MainCategory expected = expectedResultHelper(_mainCategory);
      expected.Archive();

      //ACT
      await _categoryService.DeleteCategoryAsync(MUNICIPALITYID, _mainCategory.Id);
      var actual = _dbContext.Municipalities.First().MainCategories.First();

      //ASSERT
      actual.Should().BeEquivalentTo(expected);
    }

    private MainCategory expectedResultHelper(MainCategory toBeExpected) {
      Category subCategory = new Category { Id = _mainCategory.SubCategories.First().Id, IsArchived = _mainCategory.SubCategories.First().IsArchived, Title = _mainCategory.SubCategories.First().Title };
      List<Category> subList = new List<Category> { subCategory };
      return new MainCategory { Id = _mainCategory.Id, Title = _mainCategory.Title, IsArchived = _mainCategory.IsArchived, SubCategories = subList };
    }
  }
}