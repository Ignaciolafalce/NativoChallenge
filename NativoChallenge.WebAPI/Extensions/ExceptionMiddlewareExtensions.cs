using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using NativoChallenge.WebAPI.Common;

namespace NativoChallenge.WebAPI.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
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

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(ApiResponse<object>.Failure(["Unexpected error occurred."]));
            });
        });

        return app;
    }
}