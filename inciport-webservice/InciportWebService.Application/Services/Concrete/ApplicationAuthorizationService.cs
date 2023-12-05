using InciportWebService.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class ApplicationAuthorizationService {
    private readonly IAuthorizationService _authorizationService;

    public ApplicationAuthorizationService(IAuthorizationService authorizationService) {
      _authorizationService = authorizationService;
    }

    public async Task EnsureUserAuthorizedForUserCreationWithRoleAsync(IdentityRole role, ClaimsPrincipal userClaims) {
      if (!(await IsUserAuthorizedForUserCreationWithRoleAsync(role, userClaims))) {
        throw new ForbiddenAccessException($"User was not authorized to create user with role: {role.Name}");
      }
    }

    public async Task<bool> IsUserAuthorizedForUserCreationWithRoleAsync(IdentityRole role, ClaimsPrincipal userClaims) {
      // Super User can create all roles
      if (await IsUserSuperUser(userClaims)) {
        return true;
      }

      // Elevated user can only create roles with internal access.
      if (await IsUserElevatedUser(userClaims) && role.NormalizedName == UserRoles.MANAGER) {
        return true;
      }

      return false;
    }

    public async Task<bool> IsUserSuperUser(ClaimsPrincipal claims) {
      return (await _authorizationService.AuthorizeAsync(claims, AuthorizationPolicyNames.REQUIRE_SUPER_USER)).Succeeded;
    }

    public async Task<bool> IsUserElevatedUser(ClaimsPrincipal claims) {
      return (await _authorizationService.AuthorizeAsync(claims, AuthorizationPolicyNames.REQUIRE_ELEVATED_RIGHTS)).Succeeded;
    }
  }
}