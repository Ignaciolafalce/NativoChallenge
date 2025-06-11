using NativoChallenge.Domain.Enums;
using NativoChallenge.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using NativoChallenge.Infrastructure.Data.EF;
using Entities = NativoChallenge.Domain.Entities;

namespace NativoChallenge.Infrastructure.Data.Repositories;

public class TaskRepository : GenericRepository<Entities.Task.Task, Guid>, ITaskRepository
{
    private readonly DbContextWithEvents _contextWithEvents;

    public TaskRepository(DbContextWithEvents contextWithEvents) : base(contextWithEvents.Context) { 
        _contextWithEvents = contextWithEvents;
    }

    public async Task<int> CountHighPriorityPendingAsync(CancellationToken cancellationToken)
    {
        var counter = await _dbSet.CountAsync(t => t.Priority == TaskPriority.High && t.State == TaskState.Pending, cancellationToken);
        return counter;
    }

    public async Task<Entities.Task.Task> CreateAsync(Entities.Task.Task task, CancellationToken cancellationToken)
    {
        await _contextWithEvents.Context.Set<Entities.Task.Task>().AddAsync(task);
        await _contextWithEvents.SaveChangesAsync(cancellationToken);
        return task;

    }
}
