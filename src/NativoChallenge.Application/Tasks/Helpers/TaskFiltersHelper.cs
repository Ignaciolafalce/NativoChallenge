using NativoChallenge.Domain.Enums;
using System.Linq.Expressions;
using Entities = NativoChallenge.Domain.Entities;

namespace NativoChallenge.Application.Tasks.Helpers;

public static class TaskFiltersHelper
{
    public static Expression<Func<Entities.Task, bool>> GetStateFilter(string? state)
    {
        if (string.IsNullOrWhiteSpace(state))
            return task => true;

        if (Enum.TryParse<TaskState>(state, true, out var parsedState))
            return task => task.State == parsedState;

        // If state is invalid, return a filter that matches nothing
        return task => false;
    }
}
