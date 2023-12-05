using InciportWebService.Application;
using InciportWebService.Domain;
using InciportWebService.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InciportWebService.Api {

  public class UserLoginResponseDto : GetUserDto {
    public LoginTokenDto Token { get; set; }

    public static UserLoginResponseDto FromModel(AuthorizedUser model) {
      ApplicationUser details = model.Details;
      return new UserLoginResponseDto {
        Token = new LoginTokenDto(model.LoginToken.AsTokenString(), model.LoginToken.ValidTo),
        Id = details.Id,
        FullName = details.FullName,
        Email = details.Email,
        MunicipalityId = details.MunicipalityEntityId,
        MunicipalityName = details.MunicipalityName,
        Role = details.Role
      };
    }
  }
}