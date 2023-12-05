using System.ComponentModel.DataAnnotations;

namespace InciportWebService.Application {

  public class ChosenMainCategoryDto {
    [Required]
    public int? Id { get; set; }
    public ChosenSubCategoryDto ChosenSubCategory { get; set; }
  }
}