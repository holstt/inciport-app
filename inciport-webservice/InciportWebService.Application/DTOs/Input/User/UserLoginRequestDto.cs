using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class UserLoginRequestDto {
    [Required, EmailAddress, StringLength(256)]
    public string Email { get; set; }

    [Required, StringLength(256)]
    public string Password { get; set; }
  }
}