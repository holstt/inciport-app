using InciportWebService.Domain;
using InciportWebService.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InciportWebService.Api {

  public static class ApplicationSwaggerSetup {

    public static void AddApplicationSwaggerConfig(this IServiceCollection services) {
      // Configure swagger.
      services.AddSwaggerGen(c => {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "InciportWebService.Api", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
          Description = $"JWT Authorization using the Bearer scheme." +
          $"Enter JWT Bearer token in the text input below.\n Use the demo token from top of page.",
          Name = "JWT Authorization",
          In = ParameterLocation.Header,
          Type = SecuritySchemeType.Http,
          Scheme = "Bearer",
          BearerFormat = "JWT",
        });
        // Set custom operation filter
        c.OperationFilter<AuthorizationOperationFilter>();
      });
    }

    public static void UseCustomSwaggerUI(this IApplicationBuilder app, IServiceProvider services) {
      TestDataUserFactory userFactory = new TestDataUserFactory(services);

      string maintainerToken = userFactory.CreateAndGetDemoUserTokenAsync(UserRoles.MAINTAINER).GetAwaiter().GetResult();
      string adminToken = userFactory.CreateAndGetDemoUserTokenAsync(UserRoles.ADMIN).GetAwaiter().GetResult();
      string managerToken = userFactory.CreateAndGetDemoUserTokenAsync(UserRoles.MANAGER).GetAwaiter().GetResult();

      // Create swagger page UI.
      app.UseSwagger();
      // Create simple demo user to use in swagger for endpoints that requires authorization.
      app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "InciportWebService.Api v1");
        c.HeadContent =
        $"<header style=\"background: white; width: 80%; margin: auto; color: black; word-wrap: break-word; \">" +
        $"<span style=\"font-weight: bold; font-size: 20px \">Demo tokens;</span>" +
        $"<p><b>Maintainer:</b> {maintainerToken}</p>" +
        $"<p><b>Admin:</b> {adminToken}</p>" +
        $"<p><b>Manager:</b> {managerToken}</p>" +
        $"</header>";
      });
    }
  }
}