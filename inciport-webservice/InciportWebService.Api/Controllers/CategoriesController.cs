using InciportWebService.Application;
using InciportWebService.Domain;
using InciportWebService.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InciportWebService.Api {

  [ApiController]
  public class CategoriesController : ApplicationControllerBase {
    private readonly ICategoriesService _categoriesService;

    public CategoriesController(ICategoriesService categoriesService) {
      _categoriesService = categoriesService;
    }

    [HttpGet]
    [Route("api/municipalities/{municipalityId}/categories")]
    public async Task<IActionResult> GetCategories(int municipalityId) {
      List<MainCategory> mainCategories = await _categoriesService.GetCategoriesAsync(municipalityId);
      return Ok(mainCategories.Select(m => GetMainCategoryDto.FromModel(m)));
    }

    [HttpGet]
    [Route("api/municipalities/{municipalityId}/categories/{categoryId}")]
    public async Task<IActionResult> GetCategory(int municipalityId, int categoryId) {
      MainCategory mainCategory = await _categoriesService.GetCategoryAsync(municipalityId, categoryId);
      return Ok(GetMainCategoryDto.FromModel(mainCategory));
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_ELEVATED_RIGHTS)]
    [Route("api/municipalities/{municipalityId}/categories")]
    public async Task<IActionResult> CreateCategory(int municipalityId, CreateMainCategoryDto categoryToCreate) {
      MainCategory mainCategoryCreated = await _categoriesService.CreateCategoryAsync(municipalityId, categoryToCreate);
      return Created(MakeRessourceLocationUrl(mainCategoryCreated.Id), GetMainCategoryDto.FromModel(mainCategoryCreated));
    }

    [HttpPut]
    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_ELEVATED_RIGHTS)]
    [Route("api/municipalities/{municipalityId}/categories/{categoryId}")]
    public async Task<IActionResult> UpdateCategory(int municipalityId, int categoryId, CategoryDto categoryToUpdate) {
      EnsureSameId(categoryId, categoryToUpdate.Id);
      MainCategory mainCategoryUpdated = await _categoriesService.UpdateCategoryAsync(municipalityId, categoryToUpdate);
      return Ok(GetMainCategoryDto.FromModel(mainCategoryUpdated));
    }

    [HttpDelete]
    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_ELEVATED_RIGHTS)]
    [Route("api/municipalities/{municipalityId}/categories/{categoryId}")]
    public async Task<IActionResult> DeleteCategory(int municipalityId, int categoryId) {
      await _categoriesService.DeleteCategoryAsync(municipalityId, categoryId);
      return NoContent();
    }
  }
}