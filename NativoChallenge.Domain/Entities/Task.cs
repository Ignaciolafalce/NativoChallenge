using NativoChallenge.Domain.Enums;
using NativoChallenge.Domain.Exceptions;

namespace NativoChallenge.Domain.Entities;
public class Task
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public DateTime ExpirationDate { get; private set; }
    public TaskPriority Priority { get; private set; }
    public TaskState State { get; private set; } = TaskState.Pending;

    // check if the task is expired 
    public bool IsExpired => ExpirationDate < DateTime.UtcNow;


    public Task(string title, string? description, DateTime expirationDate, TaskPriority priority)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new InvalidTaskException("The title must not be empty.");
        }

        Title = title;
        Description = description;
        ExpirationDate = expirationDate;
        Priority = priority;
    }

    // Internal Ctor for testing purposes
    internal Task(Guid id, string title, string? description, DateTime expirationDate, TaskPriority priority, string state = "Completed")
    {
        Id = id;
        Title = title;
        Description = description;
        ExpirationDate = expirationDate;
        Priority = priority;
        State = Enum.Parse<TaskState>(state);
    }

    public void Complete()
    {
        if (IsExpired)
        {
            throw new InvalidTaskException("An expirated task cannot be completed.");
        }

        State = TaskState.Completed;
    }

    // should i create another class for this type of rules? maybe...
    public static bool HighPriorityPendingLimitExceeded(int count, out string warning)
    {
        warning = "There are more than 10 high proprity pending tasks.";
        return count > 10;
    }
}
