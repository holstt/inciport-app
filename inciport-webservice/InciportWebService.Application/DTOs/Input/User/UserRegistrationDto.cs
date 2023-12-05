using InciportWebService.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class UserRegistrationDto {
    [Required, StringLength(256)]
    public string FullName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, StringLength(256)]
    public string Password { get; set; }

    [Required, StringLength(256)]
    public string Role { get; set; }

    public ApplicationUser ToModel(string municipalityName) {
      return new ApplicationUser(
        fullName: FullName,
        role: Role.ToUpper(),
        email: Email,
        municipalityName: municipalityName
      );
    }
  }
}