using NativoChallenge.Application.Tasks.DTOs;
using MediatR;

namespace NativoChallenge.Application.Tasks.Commands;

public record CreateTaskCommand(string Title, string Description, DateTime ExpirationDate, string Priority) : IRequest<CreateTaskResult>;
