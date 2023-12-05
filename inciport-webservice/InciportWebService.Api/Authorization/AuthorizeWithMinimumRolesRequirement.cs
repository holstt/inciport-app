using InciportWebService.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace InciportWebService.Api {

  /// <summary>
  /// Requires authorization for a set of role names.
  /// </summary>
  public class AuthorizeWithMinimumRolesRequirement : IAuthorizationRequirement {
    private readonly string[] _userRoles;

    public AuthorizeWithMinimumRolesRequirement(params string[] requiredRoles) {
      _userRoles = requiredRoles;
    }

    public bool HasRequiredRole(ClaimsPrincipal user) {
      return _userRoles.Any(r => user.IsInRole(r));
    }
  }
}