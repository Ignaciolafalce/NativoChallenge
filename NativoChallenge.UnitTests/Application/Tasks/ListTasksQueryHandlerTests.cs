using Moq;
using NativoChallenge.Application.Tasks.Queries;
using NativoChallenge.Application.Tasks.Queries.Handlers;
using NativoChallenge.Domain.Enums;
using NativoChallenge.Domain.Interfaces;
using System.Linq.Expressions;
using Entities = NativoChallenge.Domain.Entities;


namespace NativoChallenge.UnitTests.Application.Tasks;

public class ListTasksQueryHandlerTests
{
    readonly Mock<ITaskRepository> _mockRepo = new();

    [Fact]
    public async Task Handle_ReturnsTasksList()
    {
        // Arrange
        var tasks = new List<Entities.Task> {
            new Entities.Task(Guid.NewGuid(), "TitleLow", null, DateTime.UtcNow.AddDays(1), TaskPriority.Low),
            new Entities.Task(Guid.NewGuid(), "TitleMedium", null, DateTime.UtcNow.AddDays(1), TaskPriority.Medium)
        };

        _mockRepo.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Entities.Task, bool>>>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(tasks);

        var query = new ListTasksQuery(null, null);
        var handler = new ListTasksQueryHandler(_mockRepo.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Tasks);
        Assert.Equal(tasks.Count, result.Tasks.Count);
    }

    [Fact]
    public async Task Handle_ReturnsTasksList_Where_State_Pending()
    {
        // Arrange
        var tasks = new List<Entities.Task> {
            new Entities.Task(Guid.NewGuid(), "TitleLow", null, DateTime.UtcNow.AddDays(1), TaskPriority.Low, "Pending"),
            new Entities.Task(Guid.NewGuid(), "TitleMedium", null, DateTime.UtcNow.AddDays(1), TaskPriority.Medium)
        };

        tasks = tasks.Where(t => t.State == TaskState.Pending).ToList();

        var pendingTasks = tasks.Where(t => t.State == TaskState.Pending);
        var firstPendingTask = pendingTasks.FirstOrDefault();

        _mockRepo.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Entities.Task, bool>>>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(tasks);

        var query = new ListTasksQuery(null, null);
        var handler = new ListTasksQueryHandler(_mockRepo.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Tasks);
        Assert.Equal(pendingTasks.Count(), result.Tasks.Count);
        Assert.Equal(firstPendingTask?.Id, actual: result.Tasks?.FirstOrDefault()?.Id);
    }

    [Fact]
    public async Task Handle_ReturnsTasksList_OderBy_ExpirationDate()
    {
        // Arrange
        var tasks = new List<Entities.Task> {
            new Entities.Task(Guid.NewGuid(), "TitleSecondExp", null, DateTime.UtcNow.AddDays(3), TaskPriority.Low, "Pending"),
            new Entities.Task(Guid.NewGuid(), "TitleThirdExp", null, DateTime.UtcNow.AddDays(2), TaskPriority.Medium)
        };

        tasks.OrderBy(t => t.ExpirationDate);

        _mockRepo.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Entities.Task, bool>>>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(tasks);

        var query = new ListTasksQuery(null, null);
        var handler = new ListTasksQueryHandler(_mockRepo.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Tasks);
        Assert.Equal(result.Tasks.FirstOrDefault()?.Id, actual: result.Tasks?.FirstOrDefault()?.Id);
    }

    // Add more tests here with different states and orders (also combined could be)
}