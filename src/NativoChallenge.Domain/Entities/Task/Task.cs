using Microsoft.Extensions.Logging;
using NativoChallenge.Domain.Entities.Task.Events;
using NativoChallenge.Domain.Enums;
using NativoChallenge.Domain.Exceptions;

namespace NativoChallenge.Domain.Entities.Task;
public class Task : IHasDomainEvent
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public DateTime ExpirationDate { get; private set; }
    public TaskPriority Priority { get; private set; }
    public TaskState State { get; private set; } = TaskState.Pending;

    // check if the task is expired 
    public bool IsExpired => ExpirationDate < DateTime.UtcNow;

    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();

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

        // We can move all the logic to a factory method to create a task... that is better idea i think
        // like static Task Create(string title, string? description, DateTime expirationDate, TaskPriority priority)
        // Let's keep it simple for now because we have some reference to this constructor and no time :( 
        DomainEvents.Add(new TaskCreatedEvent(this));
        //logger

        //Log("New task instance created with ID: {@this}", this);
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
