namespace NativoChallenge.Domain.Entities.Task.Events
{
    public class TaskCreatedEvent : DomainEvent
    {
        public Task Task { get; }

        public TaskCreatedEvent(Task task)
        {
            Task = task;
        }

    }

}
