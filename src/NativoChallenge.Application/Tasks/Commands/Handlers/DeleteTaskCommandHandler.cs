using MediatR;
using NativoChallenge.Domain.Exceptions;
using NativoChallenge.Domain.Interfaces;

namespace NativoChallenge.Application.Tasks.Commands.Handlers;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
{
    private readonly ITaskRepository _taskRepository;

    public DeleteTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task Handle(DeleteTaskCommand command, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(command.TaskId, cancellationToken);

        if (task is null)
        {
            throw new InvalidTaskException($"The task with the id '{command.TaskId}' does not exist.");
        }

        await _taskRepository.DeleteAsync(command.TaskId, cancellationToken);
    }
}
