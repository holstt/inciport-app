using InciportWebService.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class UserUpdateDto {
    [Required, StringLength(256)]
    public string Id { get; set; }

    [Required, StringLength(256)]
    public string FullName { get; set; }

    [Required, StringLength(256), EmailAddress]
    public string Email { get; set; }

    [Required, StringLength(256)]
    public string Role { get; set; }
  }
}