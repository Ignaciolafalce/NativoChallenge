using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using NativoChallenge.Application.Common.Exceptions;
using NativoChallenge.Domain.Exceptions;
using NativoChallenge.WebAPI.Common;

namespace NativoChallenge.WebAPI.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        // We can refactor here...
        app.UseExceptionHandler(handler =>
        {
            handler.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                context.Response.ContentType = "application/json";

                if (exception is ValidationException validationException)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;

                    var errors = validationException.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                    var errorsList = errors.SelectMany(e => e.Value).ToList();
                    var response = ApiResponse<object>.Failure(errorsList);

                    await context.Response.WriteAsJsonAsync(response);
                    return;
                }

                if (exception is BusinessRuleException businessRuleEx)
                {
                    context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                    await context.Response.WriteAsJsonAsync(ApiResponse<object>.Failure([businessRuleEx.Message]));
                    return;
                }

                if (exception is ForbiddenAccessException forbiddenEx)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsJsonAsync(ApiResponse<object>.Failure([forbiddenEx.Message]));
                    return;
                }

                if (exception is NotFoundException notFoundEx)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsJsonAsync(ApiResponse<object>.Failure([notFoundEx.Message]));
                    return;
                }


                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(ApiResponse<object>.Failure(["Unexpected error occurred."]));
            });
        });

        return app;
    }
}