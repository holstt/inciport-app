using InciportWebService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InciportWebService.Application {

  public class CategoriesService : BaseService, ICategoriesService {
    private readonly IApplicationDbContext _dbContext;

    public CategoriesService(IApplicationDbContext dbContext) : base(dbContext) {
      _dbContext = dbContext;
    }

    public async Task<List<MainCategory>> GetCategoriesAsync(int municipalityId) {
      await EnsureMunicipalityExistsAsync(municipalityId);
      return await GetMainCategoriesAsync(municipalityId);
    }

    public async Task<MainCategory> GetCategoryAsync(int municipalityId, int categoryId) {
      await EnsureMunicipalityExistsAsync(municipalityId);
      return await GetMainCategoryAsync(municipalityId, categoryId);
    }

    public async Task<MainCategory> CreateCategoryAsync(int municipalityId, CreateMainCategoryDto categoryToCreate) {
      await EnsureMunicipalityExistsAsync(municipalityId);
      MainCategory modelCreated = categoryToCreate.ToModel();
      await SaveMainCategory(municipalityId, modelCreated);
      return modelCreated;
    }

    public async Task<MainCategory> UpdateCategoryAsync(int municipalityId, CategoryDto categoryToUpdate) {
      await EnsureMunicipalityExistsAsync(municipalityId);
      MainCategory mainCategory = await GetMainCategoryAsync(municipalityId, categoryToUpdate.Id);
      // Only title can be updated.
      mainCategory.Title = categoryToUpdate.Title;
      await _dbContext.SaveChangesAsync();
      return mainCategory;
    }

    // HELPERS

    private async Task SaveMainCategory(int municipalityId, MainCategory modelCreated) {
      _dbContext.Municipalities.FirstOrDefault(m => m.Id == municipalityId).MainCategories.Add(modelCreated);
      await _dbContext.SaveChangesAsync();
    }

    private async Task<List<MainCategory>> GetMainCategoriesAsync(int municipalityId) {
      return (await DbContext.Municipalities.Where(m => m.Id == municipalityId)
                                            .Select(m => m.MainCategories.Where(c => !c.IsArchived)).FirstOrDefaultAsync()).ToList();
    }

    public async Task<MainCategory> GetMainCategoryAsync(int municipalityId, int categoryId) {
      MainCategory mainCategory = (await DbContext.Municipalities.Where(m => m.Id == municipalityId)
                                                                .Select(m => m.MainCategories.Where(c => c.Id == categoryId && !c.IsArchived)).FirstOrDefaultAsync())
                                                                .FirstOrDefault();
      if (mainCategory is null) {
        throw new NotFoundException("Main category", categoryId);
      }

      return mainCategory;
    }

    public async Task DeleteCategoryAsync(int municipalityId, int categoryId) {
      await EnsureMunicipalityExistsAsync(municipalityId);
      MainCategory mainCategory = await GetMainCategoryAsync(municipalityId, categoryId);
      mainCategory.Archive();
      await _dbContext.SaveChangesAsync();
    }
  }
}