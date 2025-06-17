using NativoChallenge.Domain.Enums;
using NativoChallenge.Domain.Interfaces;
using MediatR;
using Entities = NativoChallenge.Domain.Entities;
using NativoChallenge.Application.Tasks.DTOs;
using NativoChallenge.Application.Tasks.Helpers;
using Microsoft.Extensions.Logging;

namespace NativoChallenge.Application.Tasks.Commands.Handlers;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, CreateTaskResult>
{
    private readonly ITaskRepository _taskRepository;
    private readonly ILogger<CreateTaskCommandHandler> _logger;

    public CreateTaskCommandHandler(ITaskRepository taskRepository, ILogger<CreateTaskCommandHandler> logger)
    {
        _taskRepository = taskRepository;
        _logger = logger;
    }

    public async Task<CreateTaskResult> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Executing CreateTaskCommandHandler for: {@command}", command);
        var (title, description, expirationDate, priorityText) = command;
        var priority = Enum.Parse<TaskPriority>(priorityText, ignoreCase: true);

        Entities.Task.Task task = new(title, description, expirationDate, priority);

        task = await _taskRepository.CreateAsync(task, cancellationToken);

        var warnings = new List<string>();
        var highPriorityPendingCount = await _taskRepository.CountHighPriorityPendingAsync(cancellationToken);
        var highPriorityPendingWarning = TaskWarningsHelpers.GetHighPriorityPendingWarningAsync(highPriorityPendingCount);
        if (highPriorityPendingWarning is not null)
        {
            warnings.Add(highPriorityPendingWarning ?? "Unknow warning.");
        }

        return new CreateTaskResult(task.Id, warnings);
    }
}
