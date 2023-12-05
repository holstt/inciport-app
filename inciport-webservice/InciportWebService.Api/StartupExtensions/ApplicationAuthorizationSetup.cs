using InciportWebService.Application;
using InciportWebService.Domain;
using InciportWebService.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Api {

  public static class ApplicationAuthorizationSetup {

    public static void AddApplicationAuthorization(this IServiceCollection services, IConfiguration configuration) {
      services.Configure<IdentityOptions>(options => {
        // Emails are used as username.
        options.User.RequireUniqueEmail = true;
      });

      services.AddIdentity<ApplicationUser, IdentityRole>()
          .AddRoles<IdentityRole>()
          .AddEntityFrameworkStores<ApplicationDbContext>()
          .AddDefaultTokenProviders();

      // Add custom AuthorizationHandler handler.
      services.AddScoped<IAuthorizationHandler, AuthorizeAlwaysHandler>();
      services.AddScoped<IAuthorizationHandler, AuthorizeIfAccessingOwnMunicipalityHandler>();

      // Define authorization policies
      services.AddAuthorization(options => {
        options.AddPolicy(AuthorizationPolicyNames.REQUIRE_INTERNAL_ACCESS, policy =>
          policy.AddRequirements(new AuthorizeWithMinimumRolesRequirement(UserRoles.MAINTAINER, UserRoles.ADMIN, UserRoles.MANAGER)));

        options.AddPolicy(AuthorizationPolicyNames.REQUIRE_ELEVATED_RIGHTS, policy =>
          policy.AddRequirements(new AuthorizeWithMinimumRolesRequirement(UserRoles.MAINTAINER, UserRoles.ADMIN)));

        options.AddPolicy(AuthorizationPolicyNames.REQUIRE_SUPER_USER, policy =>
          policy.AddRequirements(new AuthorizeWithMinimumRolesRequirement(UserRoles.MAINTAINER)));
      });

      // Authentication
      services.AddAuthentication(options => {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      // Jwt Bearer token config
      .AddJwtBearer(options => {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters() {
          ValidAudience = configuration["JWT:ValidAudience"],
          ValidIssuer = configuration["JWT:ValidIssuer"],
          ValidateAudience = false,
          ValidateIssuer = false,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
        };
      });
    }
  }
}