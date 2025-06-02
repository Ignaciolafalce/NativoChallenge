using NativoChallenge.Application.Tasks.DTOs;
using NativoChallenge.Domain.Exceptions;
using NativoChallenge.Domain.Interfaces;
using MediatR;

namespace NativoChallenge.Application.Tasks.Commands.Handlers;

public class CompleteTaskCommandHandler : IRequestHandler<CompleteTaskCommand, CompleteTaskResult>
{
    private readonly ITaskRepository _taskRepository;

    public CompleteTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<CompleteTaskResult> Handle(CompleteTaskCommand command, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(command.TaskId, cancellationToken);

        if (task is null)
        {
            throw new InvalidTaskException($"The task with the id '{command.TaskId}' does not exist.");
        }

        task.Complete();

        await _taskRepository.UpdateAsync(task, cancellationToken);

        return new CompleteTaskResult([]); // i think i should not send any warnings for the moment!
    }
}
