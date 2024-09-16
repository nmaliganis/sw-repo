using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace sw.onboarding.api.Helpers;

/// <summary>
/// Class : InternalServerErrorObjectResult
/// </summary>
public class InternalServerErrorObjectResult : ObjectResult {
    /// <summary>
    /// Ctor : InternalServerErrorObjectResult
    /// </summary>
    /// <param name="error"></param>
    public InternalServerErrorObjectResult(object error)
        : base(error) {
        this.StatusCode = StatusCodes.Status500InternalServerError;
    }
}//Class : InternalServerErrorObjectResult
