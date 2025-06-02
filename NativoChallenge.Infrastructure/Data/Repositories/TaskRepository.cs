using NativoChallenge.Domain.Enums;
using NativoChallenge.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using NativoChallenge.Infrastructure.Data.EF;
using Entities = NativoChallenge.Domain.Entities;

namespace NativoChallenge.Infrastructure.Data.Repositories;

public class TaskRepository : GenericRepository<Entities.Task, Guid>, ITaskRepository
{
    public TaskRepository(AppDbContext context) : base(context) { }

    public async Task<int> CountHighPriorityPendingAsync(CancellationToken cancellationToken)
    {
        var counter = await _dbSet.CountAsync(t => t.Priority == TaskPriority.High && t.State == TaskState.Pending, cancellationToken);
        return counter;
    }

    public async Task<Entities.Task> CreateAsync(Entities.Task task, CancellationToken cancellationToken)
    {
        return await AddAsync(task, cancellationToken); // i'm just wrapping the generic method ;), fede?
    }
}
