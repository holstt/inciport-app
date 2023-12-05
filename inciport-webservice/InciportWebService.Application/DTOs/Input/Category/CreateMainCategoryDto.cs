using InciportWebService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class CreateMainCategoryDto : CreateCategoryDto {
    public List<CreateCategoryDto> SubCategories { get; set; }

    public CreateMainCategoryDto() {
      SubCategories = new List<CreateCategoryDto>();
    }

    public MainCategory ToModel() {
      // Convert sub categories.
      List<Category> subCategories = new List<Category>();
      foreach (CreateCategoryDto dto in SubCategories) {
        subCategories.Add(new Category {
          Title = dto.Title,
        });
      }

      return new MainCategory {
        Title = Title,
        SubCategories = subCategories
      };
    }
  }
}