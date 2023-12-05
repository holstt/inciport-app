using InciportWebService.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public interface ICategoriesService {

    Task<List<MainCategory>> GetCategoriesAsync(int municipalityId);

    Task<MainCategory> GetCategoryAsync(int municipalityId, int categoryId);

    Task<MainCategory> CreateCategoryAsync(int municipalityId, CreateMainCategoryDto categoryToCreate);

    Task<MainCategory> UpdateCategoryAsync(int municipalityId, CategoryDto categoryToUpdate);

    Task DeleteCategoryAsync(int municipalityId, int categoryId);
  }
}