using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class UserPasswordUpdateDto {
    [Required, StringLength(256)]
    public string Id { get; set; }

    [Required, StringLength(256)]
    public string Password { get; set; }
  }
}