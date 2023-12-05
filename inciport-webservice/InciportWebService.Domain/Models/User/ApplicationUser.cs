using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Domain {

  public class ApplicationUser : IdentityUser {
    public string FullName { get; set; }
    public string Role { get; private set; }

    // Will be populated by EF IF saved as part of municipality context.
    public int? MunicipalityEntityId { get; set; }
    public string MunicipalityName { get; set; }

    public ApplicationUser(string fullName, string role, string email, string municipalityName = null) {
      FullName = fullName;
      UpdateRole(role, municipalityId: null);
      Role = role;
      Email = email;
      UserName = email; // Username is required by Identity.
      MunicipalityEntityId = null;
      MunicipalityName = municipalityName;
    }

    public void UpdateRole(string roleUpdate, int? municipalityId) {
      if (roleUpdate.ToUpper() == UserRoles.MAINTAINER && municipalityId != null) {
        throw new ValidationException($"Cannot create user with role '{roleUpdate}' for a municipality");
      }
      Role = roleUpdate.ToUpper();
    }

    public void UpdateUserInfo(string fullName, string email) {
      FullName = fullName;
      Email = email;
    }

    protected ApplicationUser() { // For EF
    }
  }
}