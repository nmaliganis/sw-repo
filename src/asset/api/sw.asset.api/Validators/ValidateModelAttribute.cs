﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace sw.asset.api.Validators;

/// <summary>
/// Class
/// </summary>
internal class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
            context.Result = new BadRequestObjectResult(context.ModelState);
    }

}// Class : ValidateModelAttribute