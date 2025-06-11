namespace NativoChallenge.Domain.Interfaces;

public interface ITaskRepository : IGenericRepository<Entities.Task.Task, Guid>
{
    Task<int> CountHighPriorityPendingAsync(CancellationToken cancellationToken);
    Task<Entities.Task.Task> CreateAsync(Entities.Task.Task task, CancellationToken cancellationToken);
}
