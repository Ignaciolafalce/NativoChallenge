using NativoChallenge.Domain.Interfaces;
using Entities = NativoChallenge.Domain.Entities;

namespace NativoChallenge.Application.Tasks.Helpers;

public static class TaskWarningsHelpers
{
    public static string? GetHighPriorityPendingWarningAsync(int highPriorityPendingCount)
    {
        return Entities.Task.Task.HighPriorityPendingLimitExceeded(highPriorityPendingCount, out var warning) ? warning : null;
    }
}
