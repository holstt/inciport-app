using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class CreateCategoryDto {
    [Required, StringLength(256)]
    public string Title { get; set; }
  }
}