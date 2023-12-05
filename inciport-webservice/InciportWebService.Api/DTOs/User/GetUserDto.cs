using InciportWebService.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace InciportWebService.Api {

  public class GetUserDto {
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public int? MunicipalityId { get; set; }
    public string MunicipalityName { get; set; }
    public string Role { get; set; }

    internal static GetUserDto FromModel(ApplicationUser model) {
      return new GetUserDto {
        Id = model.Id,
        FullName = model.FullName,
        Email = model.Email,
        MunicipalityId = model.MunicipalityEntityId,
        MunicipalityName = model.MunicipalityName,
        Role = model.Role
      };
    }
  }
}