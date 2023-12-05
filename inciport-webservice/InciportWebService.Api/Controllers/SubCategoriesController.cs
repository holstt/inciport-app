using InciportWebService.Application;
using InciportWebService.Domain;
using InciportWebService.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InciportWebService.Api {

  [ApiController]
  public class SubCategoriesController : ApplicationControllerBase {
    private readonly ApplicationDbContext _dbContext;

    public SubCategoriesController(ApplicationDbContext dbContext) {
      _dbContext = dbContext;
    }

    [HttpGet]
    [Route("api/municipalities/{municipalityId}/categories/{categoryId}/subcategories")]
    public async Task<IActionResult> GetCategories(int municipalityId, int categoryId) {
      MunicipalityEntity municipality = await _dbContext.Municipalities.FindAsync(municipalityId);
      if (municipality is null) {
        return NotFound("Municipality", municipalityId);
      }

      MainCategory category = municipality.MainCategories.FirstOrDefault(c => c.Id == categoryId && !c.IsArchived);
      if (category is null) {
        return NotFound("Category", categoryId);
      }

      IEnumerable<CategoryDto> subCategories = category.SubCategories.Where(s => !s.IsArchived).Select(s => CategoryDto.FromModel(s));

      return Ok(subCategories);
    }

    [HttpGet]
    [Route("api/municipalities/{municipalityId}/categories/{categoryId}/subcategories/{subcategoryId}")]
    public async Task<IActionResult> GetCategory(int municipalityId, int categoryId, int subcategoryId) {
      MunicipalityEntity municipality = await _dbContext.Municipalities.FindAsync(municipalityId);
      if (municipality is null) {
        return NotFound("Municipality", municipalityId);
      }

      MainCategory category = municipality.MainCategories.FirstOrDefault(c => c.Id == categoryId && !c.IsArchived);
      if (category is null) {
        return NotFound("Main category", categoryId);
      }

      Category subCategory = category.SubCategories.FirstOrDefault(c => c.Id == subcategoryId && !c.IsArchived);
      if (subCategory is null) {
        return NotFound("Sub category", subcategoryId);
      }

      return Ok(CategoryDto.FromModel(subCategory));
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_ELEVATED_RIGHTS)]
    [Route("api/municipalities/{municipalityId}/categories/{categoryId}/subcategories")]
    public async Task<IActionResult> CreateCategory(int municipalityId, int categoryId, [FromBody] CreateCategoryDto categoryToCreate) {
      MunicipalityEntity municipality = await _dbContext.Municipalities.FindAsync(municipalityId);
      if (municipality is null) {
        return NotFound("Municipality", municipalityId);
      }

      MainCategory mainCategory = municipality.MainCategories.FirstOrDefault(c => c.Id == categoryId && !c.IsArchived);
      if (mainCategory is null) {
        return NotFound("Main category", categoryId);
      }

      Category modelCreated = new Category {
        Title = categoryToCreate.Title
      };

      mainCategory.SubCategories.Add(modelCreated);
      await _dbContext.SaveChangesAsync();

      return Created(MakeRessourceLocationUrl(modelCreated.Id), CategoryDto.FromModel(modelCreated));
    }

    [HttpPut]
    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_ELEVATED_RIGHTS)]
    [Route("api/municipalities/{municipalityId}/categories/{categoryId}/subcategories/{subcategoryId}")]
    public async Task<IActionResult> UpdateCategory(int municipalityId, int categoryId, int subcategoryId, CategoryDto categoryToUpdate) {
      if (subcategoryId != categoryToUpdate.Id) {
        return BadRequest($"Endpoint id {subcategoryId} was different from object id {categoryToUpdate.Id}");
      }

      bool isMunicipalityFound = await _dbContext.Municipalities.AnyAsync(m => m.Id == municipalityId);
      if (!isMunicipalityFound) {
        return NotFound("Municipality", municipalityId);
      }

      // Check if exists. No tracking to avoid having two of the same instance in context
      MainCategory mainCategory = await _dbContext.MainCategories.FirstOrDefaultAsync(c => c.Id == categoryId && !c.IsArchived);
      if (mainCategory is null) {
        return NotFound("Main category", categoryId);
      }

      Category subCategory = mainCategory.SubCategories.FirstOrDefault(c => c.Id == subcategoryId && !c.IsArchived);
      if (subCategory is null) {
        return NotFound("Sub category", subcategoryId);
      }

      subCategory.Title = categoryToUpdate.Title;
      await _dbContext.SaveChangesAsync();
      return Ok(CategoryDto.FromModel(subCategory));
    }

    [HttpDelete]
    [Authorize(Policy = AuthorizationPolicyNames.REQUIRE_ELEVATED_RIGHTS)]
    [Route("api/municipalities/{municipalityId}/categories/{categoryId}/subcategories/{subcategoryId}")]
    public async Task<IActionResult> DeleteCategory(int municipalityId, int categoryId, int subcategoryId) {
      MunicipalityEntity municipality = await _dbContext.Municipalities.FindAsync(municipalityId);
      if (municipality is null) {
        return NotFound("Municipality", municipalityId);
      }

      MainCategory mainCategory = municipality.MainCategories.FirstOrDefault(c => c.Id == categoryId && !c.IsArchived);
      if (mainCategory is null) {
        return NotFound("Main category", categoryId);
      }

      Category subCategory = mainCategory.SubCategories.FirstOrDefault(c => c.Id == subcategoryId && !c.IsArchived);
      if (subCategory is null) {
        return NotFound("Sub category", subcategoryId);
      }

      subCategory.IsArchived = true;

      await _dbContext.SaveChangesAsync();
      return NoContent();
    }
  }
}