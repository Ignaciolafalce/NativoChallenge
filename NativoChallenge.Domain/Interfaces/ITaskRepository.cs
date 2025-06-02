namespace NativoChallenge.Domain.Interfaces;

public interface ITaskRepository : IGenericRepository<Entities.Task, Guid>
{
    Task<int> CountHighPriorityPendingAsync(CancellationToken cancellationToken);
    Task<Entities.Task> CreateAsync(Entities.Task task, CancellationToken cancellationToken);
}
