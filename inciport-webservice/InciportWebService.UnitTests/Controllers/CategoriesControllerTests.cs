using AutoFixture;
using FluentAssertions;
using InciportWebService.Api;
using InciportWebService.Application;
using InciportWebService.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace InciportWebService.UnitTests {

  public class CategoriesControllerTests {
    private MainCategory _mainCategory;
    private Fixture _fixture;

    public CategoriesControllerTests() {
      _fixture = new Fixture();
      _mainCategory = _fixture.Create<MainCategory>();
    }

    [Fact]
    public async Task GetCategories_ReturnsCorrectResult() {
      //ARRANGE
      const int MUNICIPALITYID = 1;

      List<MainCategory> categoryReturned = new List<MainCategory> { _mainCategory };

      List<GetMainCategoryDto> expectedCategory = new List<GetMainCategoryDto> { GetMainCategoryDto.FromModel(_mainCategory) };

      Mock<ICategoriesService> mock = new Mock<ICategoriesService>();
      mock.Setup(c => c.GetCategoriesAsync(MUNICIPALITYID)).Returns(Task.FromResult(categoryReturned));

      CategoriesController controller = new CategoriesController(mock.Object);

      //ACT
      OkObjectResult response = await controller.GetCategories(MUNICIPALITYID) as OkObjectResult;
      IEnumerable<GetMainCategoryDto> actualCategory = response.Value as IEnumerable<GetMainCategoryDto>;

      //ASSERT
      Assert.NotNull(response);
      actualCategory.Should().BeEquivalentTo(expectedCategory);
    }

    [Fact]
    public async Task GetCategory_ReturnsCorrectResult() {
      //ARRANGE
      const int MUNICIPALITYID = 1;
      const int CATEGORYID = 1;
      _mainCategory.Id = CATEGORYID;

      GetMainCategoryDto expectedCategory = GetMainCategoryDto.FromModel(_mainCategory);
      Mock<ICategoriesService> mock = new Mock<ICategoriesService>();
      mock.Setup(c => c.GetCategoryAsync(MUNICIPALITYID, CATEGORYID)).Returns(Task.FromResult(_mainCategory));
      CategoriesController controller = new CategoriesController(mock.Object);
      //ACT
      OkObjectResult response = await controller.GetCategory(MUNICIPALITYID, CATEGORYID) as OkObjectResult;
      GetMainCategoryDto actualCategory = response.Value as GetMainCategoryDto;

      //ASSERT
      Assert.NotNull(response);
      actualCategory.Should().BeEquivalentTo(expectedCategory);
    }

    [Fact]
    public async Task CreateCategory_ReturnsCorrectResult() {
      //ARRANGE
      const int MUNICIPALITYID = 1;

      Mock<ICategoriesService> mock = new Mock<ICategoriesService>();

      CreateMainCategoryDto inputCategory = new CreateMainCategoryDto {
        Title = _mainCategory.Title,
        SubCategories = _mainCategory.SubCategories.Select(x => new CreateCategoryDto { Title = x.Title }).ToList()
      };

      mock.Setup(c => c.CreateCategoryAsync(MUNICIPALITYID, inputCategory)).Returns(Task.FromResult(_mainCategory));
      var expectedCategory = GetMainCategoryDto.FromModel(_mainCategory);

      DefaultHttpContext httpContext = new DefaultHttpContext();
      httpContext.Request.Host = new HostString("host");
      httpContext.Request.Path = new PathString("/path");
      httpContext.Request.Scheme = "http";

      ControllerContext controllerContext = new ControllerContext() {
        HttpContext = httpContext,
      };
      string expectedLocationString = $"http://host/path/{_mainCategory.Id}";

      CategoriesController controller = new CategoriesController(mock.Object) { ControllerContext = controllerContext };

      //ACT
      CreatedResult response = await controller.CreateCategory(MUNICIPALITYID, inputCategory) as CreatedResult;
      string actualLocationString = response.Location;
      GetMainCategoryDto actualCategoryReturned = response.Value as GetMainCategoryDto;

      //ASSERT
      Assert.NotNull(response);
      Assert.Equal(expectedLocationString, actualLocationString);
      actualCategoryReturned.Should().BeEquivalentTo(expectedCategory);
    }

    [Fact]
    public async Task UpdateCategory_ReturnsCorrectResult() {
      //ARRANGE
      const int MUNICIPALITYID = 1;
      const int CATEGORYID = 1;
      _mainCategory.Id = CATEGORYID;

      var categoryToUpdate = new CategoryDto { Id = _mainCategory.Id, Title = _mainCategory.Title };
      GetMainCategoryDto expectedCategory = GetMainCategoryDto.FromModel(_mainCategory);

      Mock<ICategoriesService> mock = new Mock<ICategoriesService>();
      mock.Setup(c => c.UpdateCategoryAsync(MUNICIPALITYID, categoryToUpdate)).Returns(Task.FromResult(_mainCategory));
      CategoriesController controller = new CategoriesController(mock.Object);

      //ACT
      OkObjectResult response = await controller.UpdateCategory(MUNICIPALITYID, CATEGORYID, categoryToUpdate) as OkObjectResult;
      GetMainCategoryDto actualCategory = response.Value as GetMainCategoryDto;

      //ASSERT
      Assert.NotNull(response);
      actualCategory.Should().BeEquivalentTo(expectedCategory);
    }

    [Fact]
    public async Task DeleteCategory_ReturnsCorrectResult() {
      //ARRANGE
      const int MUNICIPALITYID = 1;
      const int CATEGORYID = 1;

      Mock<ICategoriesService> mock = new Mock<ICategoriesService>();
      mock.Setup(c => c.DeleteCategoryAsync(MUNICIPALITYID, CATEGORYID));
      CategoriesController controller = new CategoriesController(mock.Object);

      //ACT
      NoContentResult response = await controller.DeleteCategory(MUNICIPALITYID, CATEGORYID) as NoContentResult;

      //ASSERT
      Assert.NotNull(response);
    }
  }
}