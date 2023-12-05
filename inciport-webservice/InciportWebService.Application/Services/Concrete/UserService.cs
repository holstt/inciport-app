using InciportWebService.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class UserService : BaseService, IUserService {
    private readonly IApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationAuthorizationService _authorizationService;
    private IdentityService _identityService;

    public UserService(IApplicationDbContext dbContext,
                       UserManager<ApplicationUser> userManager,
                       RoleManager<IdentityRole> roleManager,
                       ApplicationAuthorizationService authorizationService,
                       IdentityService identityService) : base(dbContext) {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
      _authorizationService = authorizationService;
      _identityService = identityService;
    }

    public async Task<ApplicationUser> GetUserAsync(int municipalityId, string userId) {
      await EnsureMunicipalityExistsAsync(municipalityId);
      return await GetUserFromDbAsync(municipalityId, userId);
    }

    public async Task<List<ApplicationUser>> GetUsersAsync(int municipalityId) {
      await EnsureMunicipalityExistsAsync(municipalityId);
      return await GetUsersFromDbAsync(municipalityId);
    }

    public async Task DeleteUserAsync(int municipalityId, string userId) {
      await EnsureMunicipalityExistsAsync(municipalityId);
      ApplicationUser user = await GetUserFromDbAsync(municipalityId, userId);
      _dbContext.Users.Remove(user);
      await _dbContext.SaveChangesAsync();
    }

    public async Task<ApplicationUser> UpdateUserAsync(int municipalityId, UserUpdateDto userUpdate, ClaimsPrincipal userClaims) {
      await EnsureMunicipalityExistsAsync(municipalityId);
      IdentityRole roleUpdate = GetValidRole(userUpdate.Role);
      await _authorizationService.EnsureUserAuthorizedForUserCreationWithRoleAsync(roleUpdate, userClaims);

      ApplicationUser userUpdated = await _identityService.UpdateUser(userUpdate, roleUpdate);
      await _dbContext.SaveChangesAsync();
      return userUpdated;
    }

    public async Task ChangePasswordAsync(int municipalityId, UserPasswordUpdateDto userPasswordUpdate) {
      await EnsureMunicipalityExistsAsync(municipalityId);
      ApplicationUser user = await _userManager.FindByIdAsync(userPasswordUpdate.Id);
      await ChangePasswordAsync(userPasswordUpdate, user);
    }

    public async Task<ApplicationUser> CreateUserAsync(UserRegistrationDto userRegistration, int municipalityId, ClaimsPrincipal userClaims) {
      await EnsureMunicipalityExistsAsync(municipalityId);
      IdentityRole validRole = GetValidRole(userRegistration.Role);
      await _authorizationService.EnsureUserAuthorizedForUserCreationWithRoleAsync(validRole, userClaims);
      var userCreated = await _identityService.CreateUserAsync(userRegistration, validRole, await GetMunicipalityNameAsync(municipalityId));
      await AddUserToDbAsync(municipalityId, userCreated);
      return userCreated;
    }

    private IdentityRole GetValidRole(string inputRole) {
      foreach (IdentityRole role in _roleManager.Roles) {
        if (inputRole.ToUpper() == role.NormalizedName) {
          return role;
        }
      }

      throw new NotFoundException($"Role not found: {inputRole}");
    }

    private async Task AddUserToDbAsync(int municipalityId, ApplicationUser user) {
      _dbContext.Municipalities.FirstOrDefault(m => m.Id == municipalityId).Users.Add(user);
      await _dbContext.SaveChangesAsync();
    }

    private async Task<List<ApplicationUser>> GetUsersFromDbAsync(int municipalityId) {
      return (await DbContext.Municipalities.Where(m => m.Id == municipalityId)
                                            .Select(m => m.Users).FirstOrDefaultAsync())
                                            .ToList();
    }

    private async Task<string> GetMunicipalityNameAsync(int municipalityId) {
      return (await DbContext.Municipalities.Where(m => m.Id == municipalityId)
                                                          .Select(m => m.Name).FirstOrDefaultAsync());
    }

    private async Task<ApplicationUser> GetUserFromDbAsync(int municipalityId, string userId) {
      ApplicationUser user = await _userManager.FindByIdAsync(userId);

      if (user is null || user.MunicipalityEntityId != municipalityId) {
        throw new NotFoundException("User", userId);
      }
      return user;
    }

    private async Task ChangePasswordAsync(UserPasswordUpdateDto userPasswordUpdate, ApplicationUser user) {
      // Update password
      string token = await _userManager.GeneratePasswordResetTokenAsync(user);
      IdentityResult registrationResult = await _userManager.ResetPasswordAsync(user, token, userPasswordUpdate.Password);
      // Check for errors i.e. password rules not satisfied.
      if (!registrationResult.Succeeded) {
        throw new ValidationException("Update password", registrationResult.Errors.Select(e => e.Description).ToList());
      }
    }
  }
}