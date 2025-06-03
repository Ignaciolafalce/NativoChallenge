using NativoChallenge.Application.Tasks.DTOs;
using MediatR;

namespace NativoChallenge.Application.Tasks.Commands;

public record CompleteTaskCommand(Guid TaskId) : IRequest<CompleteTaskResult>;
