using System.Linq.Expressions;
using Entities = NativoChallenge.Domain.Entities;

namespace NativoChallenge.Application.Tasks.Helpers;

public static class TaskFiltersHelper
{
    public static Expression<Func<Entities.Task, bool>> GetStateFilter(string? state)
    {
        return task => string.IsNullOrWhiteSpace(state) || task.State.ToString().Equals(state, StringComparison.OrdinalIgnoreCase);
    }
}
