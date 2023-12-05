using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InciportWebService.Api {

  /// <summary>
  /// Custom Swagger UI filter that adds a padlock icon on authorized endpoints only.
  /// </summary>
  public class AuthorizationOperationFilter : IOperationFilter {

    public void Apply(OpenApiOperation operation, OperationFilterContext context) {
      if (true) {
        OpenApiSecurityRequirement securityRequirement = new OpenApiSecurityRequirement(){
          {
            new OpenApiSecurityScheme {
              Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,
            },
            new List<string>()
          }
        };

        operation.Security = new List<OpenApiSecurityRequirement> { securityRequirement };
        operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
      }
    }
  }
}