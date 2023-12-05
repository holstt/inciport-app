using InciportWebService.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class ChosenMainCategoryEntity {
    [Required]
    public int MainCategoryId { get; set; }
    public int? SubCategoryId { get; set; }

    public static ChosenMainCategoryEntity FromChosenCategory(ChosenMainCategory chosenMainCategory) {
      if (chosenMainCategory is null) {
        return null;
      }

      return new ChosenMainCategoryEntity {
        MainCategoryId = chosenMainCategory.Id,
        SubCategoryId = chosenMainCategory.ChosenSubCategory?.Id
      };
    }

    public static ChosenMainCategoryEntity FromCategory(MainCategory mainCategoryOption, int? selectedSubCategoryId = null) {
      Category subCategoryOption = null;
      if (selectedSubCategoryId != null) {
        // Throws if not exist.
        subCategoryOption = mainCategoryOption.SubCategories.FirstOrDefault(c => c.Id == selectedSubCategoryId);
        if (subCategoryOption is null) {
          throw new InvalidOperationException($"{nameof(selectedSubCategoryId)} {selectedSubCategoryId} was specified, but did not exist in {nameof(mainCategoryOption.SubCategories)}.");
        }
      }
      // Create category model.
      return new ChosenMainCategoryEntity {
        MainCategoryId = mainCategoryOption.Id,
        SubCategoryId = subCategoryOption?.Id
      };
    }
  }
}