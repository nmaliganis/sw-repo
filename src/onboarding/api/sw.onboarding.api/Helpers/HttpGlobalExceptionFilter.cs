﻿using System.Net;
using sw.auth.api.Helpers;
using sw.auth.common.infrastructure.Exceptions;
using sw.onboarding.common.infrastructure.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace sw.onboarding.api.Helpers;

/// <summary>
/// Class : HttpGlobalExceptionFilter
/// </summary>
public class HttpGlobalExceptionFilter : IExceptionFilter {
  private readonly IWebHostEnvironment _env;
  private readonly ILogger<HttpGlobalExceptionFilter> _logger;

  /// <summary>
  /// Ctor : HttpGlobalExceptionFilter
  /// </summary>
  /// <param name="env"></param>
  /// <param name="logger"></param>
  public HttpGlobalExceptionFilter(IWebHostEnvironment env, ILogger<HttpGlobalExceptionFilter> logger) {
    this._env = env;
    this._logger = logger;
  }

  /// <summary>
  /// Method : OnException
  /// </summary>
  /// <param name="context"></param>
  public void OnException(ExceptionContext context) {
    this._logger.LogError(new EventId(context.Exception.HResult),
      context.Exception,
      context.Exception.Message);

    if (context.Exception.GetType() == typeof(AuthDomainException)) {
      var problemDetails = new ValidationProblemDetails() {
        Instance = context.HttpContext.Request.Path,
        Status = StatusCodes.Status400BadRequest,
        Detail = "Please refer to the errors property for additional details."
      };

      problemDetails.Errors.Add("DomainValidations", new string[] { context.Exception.Message.ToString() });

      context.Result = new BadRequestObjectResult(problemDetails);
      context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    } else {
      var json = new JsonErrorResponse {
        Messages = new[] { "An error occur.Try it again." }
      };

      if (this._env.IsDevelopment()) {
        json.DeveloperMessage = context.Exception;
      }

      context.Result = new InternalServerErrorObjectResult(json);
      context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    }
    context.ExceptionHandled = true;
  }

  private class JsonErrorResponse {
    public string[] Messages { get; set; }

    public object DeveloperMessage { get; set; }
  }
}