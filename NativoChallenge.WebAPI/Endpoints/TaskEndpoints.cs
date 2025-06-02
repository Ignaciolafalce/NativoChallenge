using MediatR;
using Microsoft.AspNetCore.Mvc;
using NativoChallenge.Application.Tasks.Commands;
using NativoChallenge.Application.Tasks.DTOs;
using NativoChallenge.Application.Tasks.Queries;
using NativoChallenge.WebAPI.Common;

namespace NativoChallenge.WebAPI.Endpoints;

public static class TaskEndpoints
{
    public static IEndpointRouteBuilder MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/tasks");

        // GET /tasks?state=Pending&orderBy=priority
        group.MapGet("/", async (ISender sender, [AsParameters] ListTasksQuery query) =>
        {
            var result = await sender.Send(query);
            return Results.Ok(ApiResponse<ListTasksResult>.Success(result));
        });

        // POST /tasks
        group.MapPost("/", async (ISender sender, [FromBody] CreateTaskCommand command) =>
        {
            var result = await sender.Send(command);
            return Results.Ok(ApiResponse<CreateTaskResult>.Success(result));
        });

        // DELETE /tasks/{id}
        group.MapDelete("/{id:guid}", async (ISender sender, [FromRoute] Guid id) =>
        {
            await sender.Send(new DeleteTaskCommand(id));
            return Results.NoContent();
        });

        // PUT /tasks/{id}/complete
        group.MapPut("/{id:guid}/complete", async (ISender sender, [FromRoute] Guid id) =>
        {
            await sender.Send(new CompleteTaskCommand(id));
            return Results.NoContent();
        });

        return app;
    }
}
