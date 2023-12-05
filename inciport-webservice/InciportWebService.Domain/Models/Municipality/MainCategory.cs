using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InciportWebService.Domain {

  public class MainCategory : Category {
    public virtual List<Category> SubCategories { get; set; }

    /// <summary>
    /// Archives the category and all of it sub categories.
    /// </summary>
    public void Archive() {
      IsArchived = true;
      foreach (var subCategory in SubCategories) {
        subCategory.IsArchived = true;
      }
    }

    /// <summary>
    /// Creates a <see cref="ChosenMainCategory"/> model from this instance with the specified sub category if any.
    /// </summary>
    /// <param name="selectedSubCategoryId"></param>
    /// <returns></returns>
    public ChosenMainCategory ToChosenMainCategory(int? selectedSubCategoryId = null) {
      Category subCategory = null;
      if (selectedSubCategoryId != null) {
        // Throws if not exist.
        Category subCategoryOption = SubCategories.FirstOrDefault(c => c.Id == selectedSubCategoryId);
        if (subCategoryOption is null) {
          throw new InvalidOperationException($"{nameof(selectedSubCategoryId)} {selectedSubCategoryId} was specified, but did not exist in {nameof(SubCategories)}.");
        }

        subCategory = new Category {
          Id = subCategoryOption.Id,
          Title = subCategoryOption.Title,
        };
      }
      // Create category model.
      return new ChosenMainCategory {
        Id = Id,
        Title = Title,
        ChosenSubCategory = subCategory
      };
    }
  }
}