using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using InciportWebService.Application;
using Microsoft.AspNetCore.Http;
using InciportWebService.Domain;

namespace InciportWebService.Api {

  /// <summary>
  /// Catches custom application exceptions and translates them to HTTP responses.
  /// </summary>
  public class ApplicationExceptionFilter : IActionFilter {

    public void OnActionExecuted(ActionExecutedContext context) {
      switch (context.Exception) {
        case NotFoundException e: {
            HandleNotFoundException(context, e);
            return;
          }

        case ValidationException e: {
            HandleValidationException(context, e);
            return;
          }

        case ForbiddenAccessException e:
          HandleForbiddenAccessException(context, e);
          return;

        default:
          break;
      }
    }

    private void HandleForbiddenAccessException(ActionExecutedContext context, ForbiddenAccessException e) {
      ProblemDetails details = new ProblemDetails() {
        Status = StatusCodes.Status403Forbidden,
        Title = "Forbidden",
        Detail = e.Message
      };

      context.Result = new ObjectResult(details) {
        StatusCode = StatusCodes.Status403Forbidden
      };
      context.ExceptionHandled = true;
    }

    private void HandleValidationException(ActionExecutedContext context, ValidationException e) {
      ProblemDetails problemDetails;

      if (e.Errors.Count > 1) {
        Dictionary<string, string[]> errors = new Dictionary<string, string[]>();
        errors.Add(e.Title, e.Errors.ToArray());
        problemDetails = new ValidationProblemDetails(errors);
      }
      else {
        problemDetails = new ProblemDetails() {
          Detail = e.Errors.First()
        };
      }

      problemDetails.Status = StatusCodes.Status400BadRequest;
      problemDetails.Title = "Bad Request";

      context.Result = new BadRequestObjectResult(problemDetails);
      context.ExceptionHandled = true;
    }

    private static void HandleNotFoundException(ActionExecutedContext context, NotFoundException e) {
      ProblemDetails details = new ProblemDetails() {
        Status = StatusCodes.Status404NotFound,
        Title = "Not Found",
        Detail = e.Message
      };

      context.Result = new NotFoundObjectResult(details);
      context.ExceptionHandled = true;
    }

    public void OnActionExecuting(ActionExecutingContext context) {
    }
  }
}