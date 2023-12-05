using InciportWebService.Domain;
using InciportWebService.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InciportWebService.Api {

  /// <summary>
  /// Requires that a user is accessing a ressource for its' own municipality.
  /// </summary>
  public class AuthorizeIfAccessingOwnMunicipalityHandler : AuthorizationHandler<AuthorizeWithMinimumRolesRequirement> {
    private readonly string[] VAILID_ROLES = { UserRoles.ADMIN, UserRoles.MANAGER };

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _dbContext;

    public AuthorizeIfAccessingOwnMunicipalityHandler(UserManager<ApplicationUser> userManager,
                                            ApplicationDbContext dbContext) {
      _userManager = userManager;
      _dbContext = dbContext;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizeWithMinimumRolesRequirement requirement) {
      if (!requirement.HasRequiredRole(context.User) || !VAILID_ROLES.Any(r => context.User.IsInRole(r))) {
        return;
      }

      int municipalityId = GetMunicipalityId(context);

      if (await IsUserFromMunicipality(context.User, municipalityId, _userManager)) {
        context.Succeed(requirement);
      }
    }

    private int GetMunicipalityId(AuthorizationHandlerContext context) {
      HttpContext httpContext = context.Resource as HttpContext;
      if (httpContext is null) {
        throw new ArgumentException($"{nameof(HandleRequirementAsync)} was called with empty {nameof(HttpContext)}");
      }

      object municipalityIdRouteValue;
      bool hasMunicipalityId = httpContext.Request.RouteValues.TryGetValue("municipalityId", out municipalityIdRouteValue);
      if (!hasMunicipalityId) {
        throw new ArgumentException($"{context} did not contain a municipality id");
      }

      int municipalityId;
      bool isNumber = int.TryParse(municipalityIdRouteValue.ToString(), out municipalityId);
      if (!isNumber) {
        throw new ArgumentException($"Unable to convert municipality id to an integer");
      }

      return municipalityId;
    }

    public async Task<bool> IsUserFromMunicipality(ClaimsPrincipal user, int municipalityId, UserManager<ApplicationUser> userManager) {
      ApplicationUser appUser = await GetUser(user, userManager);
      return appUser.MunicipalityEntityId == municipalityId;
    }

    private async Task<ApplicationUser> GetUser(ClaimsPrincipal user, UserManager<ApplicationUser> userManager) {
      string userName = user.FindFirst(ClaimTypes.Name).Value;
      ApplicationUser appUser = await userManager.FindByNameAsync(userName);
      return appUser;
    }
  }
}