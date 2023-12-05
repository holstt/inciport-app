using InciportWebService.Application;
using InciportWebService.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Data {

  public class TestDataUserFactory {
    private IServiceProvider _services;
    private AuthencicationService _authencicationService;
    //private IUserService _userService;
    private IdentityService _identityService;

    public TestDataUserFactory(IServiceProvider services) {
      _services = services;
      _authencicationService = _services.GetRequiredService<AuthencicationService>();
      _identityService = _services.GetRequiredService<IdentityService>();
      //_userService = _services.GetRequiredService<IUserService>();
    }

    public async Task<string> CreateAndGetDemoUserTokenAsync(string role) {
      ApplicationDbContext dbContext = _services.GetRequiredService<ApplicationDbContext>();

      UserRegistrationDto demoUser = new UserRegistrationDto {
        FullName = $"Demo User {role}",
        Email = $"{role.ToLower()}@test.com",
        Password = "test",
        Role = role,
      };

      MunicipalityEntity municipalityForTestUsers = dbContext.Municipalities.First();

      try {
        ApplicationUser createdUser = await _identityService.CreateUserAsync(demoUser, new IdentityRole(demoUser.Role), demoUser.Role == UserRoles.MAINTAINER ? null : municipalityForTestUsers.Name);

        // Demo users for maintainer roles should be associated to a municipality.
        // NB: Do not try saving maintainers to db.users -> This will fail as it is already done by identity i.e. only adding works.
        if (role != UserRoles.MAINTAINER) {
          municipalityForTestUsers.Users.Add(createdUser);
        }

        dbContext.SaveChanges();
      }
      catch (ValidationException) {
      }

      AuthorizedUser authorizedUser = await _authencicationService.LoginAsync(demoUser.Email, demoUser.Password);
      return new JwtSecurityTokenHandler().WriteToken(authorizedUser.LoginToken);
    }

    public async Task<string> CreateMaintainerUserAsync(List<ApplicationUser> users) {
      UserRegistrationDto maintainer = new UserRegistrationDto {
        FullName = $"Martin Maintainer",
        Email = "maintainer-test@test.com",
        Password = "test",
        Role = UserRoles.MAINTAINER,
      };

      return await CreateTestUser(users, maintainer, municipalityName: null);
    }

    public async Task<string> CreateAdminUserAsync(MunicipalityEntity municipality) {
      UserRegistrationDto admin = new UserRegistrationDto {
        FullName = $"Anders Admin {municipality.Name}",
        Email = $"admin-{municipality.Name.ToLower()}@test.com",
        Password = "test",
        Role = UserRoles.ADMIN,
      };

      return await CreateTestUser(municipality.Users, admin, municipality.Name);
    }

    public async Task<string> CreateManagerUserAsync(MunicipalityEntity municipality) {
      UserRegistrationDto manager = new UserRegistrationDto {
        FullName = $"Marianne Manager {municipality.Name}",
        Email = $"manager-{municipality.Name.ToLower()}@test.com",
        Password = "test",
        Role = UserRoles.MANAGER,
      };

      return await CreateTestUser(municipality.Users, manager, municipality.Name);
    }

    private async Task<string> CreateTestUser(List<ApplicationUser> usersToAddTo, UserRegistrationDto userToCreate, string municipalityName) {
      ApplicationUser userCreated = await _identityService.CreateUserAsync(userToCreate, new IdentityRole(userToCreate.Role), municipalityName);
      usersToAddTo.Add(userCreated); // This is required in order to assign an id to the user.
      AuthorizedUser authorizedUser = await _authencicationService.LoginAsync(userToCreate.Email, userToCreate.Password);
      return new JwtSecurityTokenHandler().WriteToken(authorizedUser.LoginToken);
    }
  }
}