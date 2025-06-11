using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NativoChallenge.Application.Auth.Commands;
using NativoChallenge.Application.Auth.DTOs;
using NativoChallenge.WebAPI.Common;

namespace NativoChallenge.WebAPI.Endpoints
{
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {

            var group = app.MapGroup("/auth");

            group.MapPost("/token", async (ISender sender, [FromBody] TokenCommand command) =>
            {
                var result = await sender.Send(command);
                return Results.Ok(ApiResponse<TokenResult>.Success(result));
            });

            return app;

        }
    }
}
