using InciportWebService.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace InciportWebService.Application {

  public class CategoryDto {
    [Required]
    public int Id { get; set; }

    [Required, StringLength(256)]
    public string Title { get; set; }

    public static CategoryDto FromModel(Category model) {
      return new CategoryDto { Id = model.Id, Title = model.Title };
    }
  }
}