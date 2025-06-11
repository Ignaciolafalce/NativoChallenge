using NativoChallenge.Domain.Entities.Task.Events;
using NativoChallenge.Domain.Enums;
namespace NativoChallenge.UnitTests.Domain.Tasks
{
    public class TaskDomainEventTests
    {
        [Fact]
        public void Constuctor_ShouldInitializeDomainEvent_WhenTaskIsCreated()
        {
            // Arrange
            var title = "Test Task";
            var description = "This is a test task.";
            var expirationDate = DateTime.UtcNow.AddDays(1);
            var priority = TaskPriority.High;

            // Act
            var task = new NativoChallenge.Domain.Entities.Task.Task(title, description, expirationDate, priority);
            var taskCreatedEvent = task.DomainEvents.OfType<TaskCreatedEvent>().FirstOrDefault();

            // Assert
            Assert.NotNull(task.DomainEvents);
            Assert.NotNull(taskCreatedEvent);
            Assert.IsType<TaskCreatedEvent>(taskCreatedEvent);
        }

    }
}
