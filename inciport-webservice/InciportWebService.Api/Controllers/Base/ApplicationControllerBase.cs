using InciportWebService.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InciportWebService.Api {

  /// <summary>
  /// Provides useful base methods for all controllers.
  /// </summary>
  public class ApplicationControllerBase : ControllerBase {

    // Take into account proxy forward header
    private string GetScheme() {
      StringValues schemeForwardedByProxy;
      if (Request.Headers.TryGetValue("X-Forwarded-Proto", out schemeForwardedByProxy)) {
        return schemeForwardedByProxy;
      }

      return Request.Scheme;
    }

    protected Uri GetRequestBaseUrl() => new Uri($"{GetScheme()}://{Request.Host}");

    protected Uri GetFullRequestUrl() => new Uri(new Uri($"{GetScheme()}://{Request.Host}"), Request.Path.ToString() + "/"); // Combine two uris

    /// <summary>
    /// Makes a url of a ressource with the specified id based on the current request path.
    /// </summary>
    protected Uri MakeRessourceLocationUrl(int ressourceId) => MakeRessourceLocationUrl(ressourceId.ToString());

    protected Uri MakeRessourceLocationUrl(string ressourceId) => new Uri(GetFullRequestUrl(), ressourceId);

    protected ObjectResult NotFound(string ressourceName, int ressourceId) => NotFound(ressourceName, ressourceId.ToString());

    protected ObjectResult NotFound(string ressourceName, string ressourceId) {
      return Problem(
        detail: $"{ressourceName} with id '{ressourceId}' was not found",
        statusCode: StatusCodes.Status404NotFound
      );
    }

    protected ObjectResult Forbid(string errorMessage) => CreateDefaultProblem(errorMessage, StatusCodes.Status403Forbidden);

    protected ObjectResult BadRequest(string errorMessage) => CreateDefaultProblem(errorMessage, StatusCodes.Status400BadRequest);

    protected ObjectResult Unauthorized(string errorMessage) => CreateDefaultProblem(errorMessage, StatusCodes.Status401Unauthorized);

    protected ObjectResult CreateDefaultProblem(string errorMessage, int statusCode) {
      return Problem(
        detail: errorMessage,
        statusCode: statusCode
      );
    }

    protected static void EnsureSameId(string endpointId, string objectId) {
      if (endpointId != objectId) {
        throw new ValidationException($"Endpoint id '{endpointId}' was different from object id '{objectId}'");
      }
    }

    protected static void EnsureSameId(int endpointId, int objectId) {
      if (endpointId != objectId) {
        throw new ValidationException($"Endpoint id '{endpointId}' was different from object id '{objectId}'");
      }
    }
  }
}