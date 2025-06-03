using MediatR;

namespace NativoChallenge.Application.Tasks.Commands;

public record DeleteTaskCommand(Guid TaskId) : IRequest;
