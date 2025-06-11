using Entities = NativoChallenge.Domain.Entities;

namespace NativoChallenge.Application.Tasks.Helpers;
public static class TaskOrderingHelper
{
    private static readonly Dictionary<string, Func<IQueryable<Entities.Task.Task>, IOrderedQueryable<Entities.Task.Task>>> _orderBySelectors =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { nameof(Entities.Task.Task.ExpirationDate), query => query.OrderBy(task => task.ExpirationDate) },

            { nameof(Entities.Task.Task.Priority), query => query.OrderBy(task => task.Priority) }
        };

    public static Func<IQueryable<Entities.Task.Task>, IOrderedQueryable<Entities.Task.Task>> GetOrderFunc(string? orderBy)
    {
        var key = orderBy ?? string.Empty;

        // the default order is by Id
        return _orderBySelectors.GetValueOrDefault(key, query => query.OrderBy(task => task.Id));
    }
}
