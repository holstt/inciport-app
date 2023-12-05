using AutoFixture;
using FluentAssertions;
using InciportWebService.Api;
using InciportWebService.Application;
using InciportWebService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InciportWebService.UnitTests {

  public class CategoryDtoTests {
    private Fixture _fixture;

    public CategoryDtoTests() {
      _fixture = new Fixture();
    }

    [Fact]
    public void GetMainCategoryDtoFromModel_WithSubCategories() {
      //ARRANGE
      List<Category> subCategories = new List<Category> { new Category { Id = 1, Title = "Title", IsArchived = false } };
      MainCategory input = new MainCategory { Id = 1, Title = "Title", IsArchived = false, SubCategories = subCategories };

      List<CategoryDto> subCategoriesDto = new List<CategoryDto> { new CategoryDto { Id = 1, Title = "Title" } };
      GetMainCategoryDto expectedResult = new GetMainCategoryDto { Id = 1, Title = "Title", SubCategories = subCategoriesDto };

      //ACT
      GetMainCategoryDto actualResult = GetMainCategoryDto.FromModel(input);

      //ASSERT
      actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void CategoryDtoFromModel() {
      //ARRANGE
      Category inputModel = new Category { Id = 1, Title = "Title", IsArchived = false };
      CategoryDto expectedResult = new CategoryDto { Id = 1, Title = "Title" };

      //ACT
      var actualResult = CategoryDto.FromModel(inputModel);

      //ASSERT
      actualResult.Should().BeEquivalentTo(expectedResult);
    }
  }
}