using InciportWebService.Domain;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InciportWebService.Api {

  /// <summary>
  /// Handles authorization for Maintainers.
  /// </summary>
  public class AuthorizeAlwaysHandler : AuthorizationHandler<AuthorizeWithMinimumRolesRequirement> {
    private readonly string[] VAILID_ROLES = { UserRoles.MAINTAINER }; // Only the Maintainer is always authorized.

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizeWithMinimumRolesRequirement requirement) {
      if (!requirement.HasRequiredRole(context.User)) {
        return Task.CompletedTask;
      }
      else if (VAILID_ROLES.Any(r => context.User.IsInRole(r))) {
        context.Succeed(requirement);
      }
      return Task.CompletedTask;
    }
  }
}