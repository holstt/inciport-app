using InciportWebService.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Data {

  public class DbDefaultInitializer {
    private readonly ApplicationDbContext _dbContext;
    private readonly IServiceProvider _services;
    private readonly IConfiguration _configuration;

    public DbDefaultInitializer(ApplicationDbContext dbContext, IServiceProvider services, IConfiguration configuration) {
      _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
      _services = services;
      _configuration = configuration;
    }

    public async Task InitializeAsync() {
      Console.WriteLine("Initializing DEFAULT DB...");
      await _dbContext.ResetDbAsync();
      RoleManager<IdentityRole> roleManager = _services.GetRequiredService<RoleManager<IdentityRole>>();
      UserManager<ApplicationUser> userManager = _services.GetRequiredService<UserManager<ApplicationUser>>();
      await SeedRolesAsync(roleManager);
      await SeedDefaultUserAsync(userManager);
      _dbContext.SaveChanges();
      Console.WriteLine("Db initialized!");
    }

    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager) {
      // Define roles
      string[] roleNames = { UserRoles.MAINTAINER, UserRoles.ADMIN, UserRoles.MANAGER };

      // Create roles
      IdentityResult roleResult;
      foreach (string roleName in roleNames) {
        bool roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist) {
          // Create new role
          Console.WriteLine("Creating role that did not exist: " + roleName);
          roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
        }
      }
    }

    private async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager) {
      // Create maintainer user
      ApplicationUser maintainerUser = new ApplicationUser(
        fullName: "Didrik Default",
        role: UserRoles.MAINTAINER,
        email: _configuration["DefaultLogin:Email"]
      );

      string maintainerPassword = _configuration["DefaultLogin:Password"];
      await SeedUserAsync(userManager, maintainerUser, maintainerPassword);
    }

    /// <summary>
    /// Creates an initial user in the database.
    /// </summary>
    /// <returns></returns>
    public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, ApplicationUser user, string password) {
      ApplicationUser userExist = await userManager.FindByEmailAsync(user.Email);

      if (userExist is null) {
        IdentityResult result = await userManager.CreateAsync(user, password);
        if (result.Succeeded) {
          await userManager.AddToRoleAsync(user, user.Role);
        }
      }
    }
  }
}