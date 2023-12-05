using InciportWebService.Application;
using InciportWebService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InciportWebService.Api {

  public class GetChosenMainCategoryDto : CategoryDto {
    public CategoryDto ChosenSubCategory { get; set; }

    internal static GetChosenMainCategoryDto FromModel(ChosenMainCategory model) {
      return new GetChosenMainCategoryDto {
        Id = model.Id,
        Title = model.Title,
        ChosenSubCategory = model.ChosenSubCategory != null ? FromModel(model.ChosenSubCategory) : null
      };
    }
  }
}