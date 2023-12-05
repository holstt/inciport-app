using InciportWebService.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class IdentityService {
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(UserManager<ApplicationUser> userManager) {
      _userManager = userManager;
    }

    public async Task<ApplicationUser> CreateUserAsync(UserRegistrationDto userRegistration, IdentityRole role, string municipalityName) {
      await EnsureEmailNotTaken(userRegistration.Email);
      return await CreateUserWithRole(userRegistration, role, municipalityName);
    }

    public async Task<ApplicationUser> UpdateUser(UserUpdateDto userUpdate, IdentityRole roleUpdate) {
      ApplicationUser userExisting = await _userManager.FindByIdAsync(userUpdate.Id);

      if (userExisting is null) {
        throw new NotFoundException("User", userUpdate.Id);
      }

      await UpdateUserRole(userExisting, roleUpdate);
      await UpdateUserInfo(userUpdate, userExisting);
      return userExisting;
    }

    private async Task UpdateUserRole(ApplicationUser existingUser, IdentityRole roleUpdate) {
      Console.WriteLine("Updating role...");
      await RemoveRole(existingUser);
      existingUser.UpdateRole(roleUpdate.Name, existingUser.MunicipalityEntityId.Value);
      await AddRole(existingUser, roleUpdate);
    }

    private async Task UpdateUserInfo(UserUpdateDto userUpdate, ApplicationUser existingUser) {
      // Update  info
      existingUser.UpdateUserInfo(userUpdate.FullName, userUpdate.Email);
      IdentityResult updateUserResult = await _userManager.UpdateAsync(existingUser);
      if (!updateUserResult.Succeeded) {
        throw new ValidationException("Update info", updateUserResult.Errors.Select(e => e.Description).ToList());
      }
    }

    private async Task RemoveRole(ApplicationUser existingUser) {
      IdentityResult removeRoleResult = await _userManager.RemoveFromRoleAsync(existingUser, existingUser.Role);
      if (!removeRoleResult.Succeeded) {
        throw new ValidationException("Update role", removeRoleResult.Errors.Select(e => e.Description).ToList());
      }
    }

    private async Task AddRole(ApplicationUser existingUser, IdentityRole roleUpdate) {
      IdentityResult addRoleResult = await _userManager.AddToRoleAsync(existingUser, roleUpdate.Name);
      if (!addRoleResult.Succeeded) {
        throw new ValidationException("Update role", addRoleResult.Errors.Select(e => e.Description).ToList());
      }
    }

    private async Task EnsureEmailNotTaken(string email) {
      ApplicationUser userExisting = await _userManager.FindByEmailAsync(email);
      if (userExisting is not null) {
        throw new ValidationException($"User {email} already exists");
      }
    }

    private async Task<ApplicationUser> CreateUserWithRole(UserRegistrationDto userRegistration, IdentityRole role, string municipalityName) {
      ApplicationUser userCreated = userRegistration.ToModel(municipalityName);
      await CreateUser(userCreated, userRegistration.Password);
      await AddRole(userCreated, role);
      return userCreated;
    }

    private async Task CreateUser(ApplicationUser user, string password) {
      IdentityResult registrationResult = await _userManager.CreateAsync(user, password);

      // Check for errors i.e. password rules not satisfied.
      if (!registrationResult.Succeeded) {
        throw new ValidationException("Create user", registrationResult.Errors.Select(e => e.Description).ToList());
      }
    }
  }
}