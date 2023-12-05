using InciportWebService.Application;
using InciportWebService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InciportWebService.Api {

  public class GetMainCategoryDto : CategoryDto {
    public List<CategoryDto> SubCategories { get; set; }

    public static GetMainCategoryDto FromModel(MainCategory model) {
      return new GetMainCategoryDto() {
        Id = model.Id,
        SubCategories = model.SubCategories.Select(c => FromModel(c)).ToList(),
        Title = model.Title
      };
    }
  }
}