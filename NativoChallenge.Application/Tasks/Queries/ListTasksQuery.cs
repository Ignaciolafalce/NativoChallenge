using NativoChallenge.Application.Tasks.DTOs;
using MediatR;

namespace NativoChallenge.Application.Tasks.Queries;

public record ListTasksQuery(string? State, string? OrderBy) : IRequest<ListTasksResult>;
