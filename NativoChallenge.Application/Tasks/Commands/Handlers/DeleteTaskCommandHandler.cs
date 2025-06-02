using NativoChallenge.Domain.Interfaces;
using MediatR;

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
        await _taskRepository.DeleteAsync(command.TaskId, cancellationToken);
    }
}
