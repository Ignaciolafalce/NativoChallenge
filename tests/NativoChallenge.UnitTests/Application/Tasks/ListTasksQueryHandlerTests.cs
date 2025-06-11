using AutoMapper;
using Moq;
using NativoChallenge.Application.Tasks.Mapping;
using NativoChallenge.Application.Tasks.Queries;
using NativoChallenge.Application.Tasks.Queries.Handlers;
using NativoChallenge.Domain.Entities.Task;
using NativoChallenge.Domain.Enums;
using NativoChallenge.Domain.Interfaces;
using System.Linq.Expressions;
using Entities = NativoChallenge.Domain.Entities.Task;
using Threading = System.Threading.Tasks;


namespace NativoChallenge.UnitTests.Application.Tasks;

public class ListTaskQueryHandlerTestFixture : IDisposable
{
    public IMapper Mapper { get; private set; }
    public ListTaskQueryHandlerTestFixture()
    {
        Mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TaskDtoProfile>();

        }).CreateMapper();
    }


    public void Dispose(){}
}

public class ListTasksQueryHandlerTests : IClassFixture<ListTaskQueryHandlerTestFixture>
{

    readonly Mock<ITaskRepository> _mockRepo = new();
    private readonly ListTaskQueryHandlerTestFixture _fixture;

    public ListTasksQueryHandlerTests(ListTaskQueryHandlerTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Threading.Task Handle_ReturnsTasksList()
    {
        // Arrange
        var tasks = new List<Entities.Task> {
            new Entities.Task(Guid.NewGuid(), "TitleLow", null, DateTime.UtcNow.AddDays(1), TaskPriority.Low),
            new Entities.Task(Guid.NewGuid(), "TitleMedium", null, DateTime.UtcNow.AddDays(1), TaskPriority.Medium)
        };

        _mockRepo.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Entities.Task, bool>>>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(tasks);

        var query = new ListTasksQuery(null, null);
        var handler = new ListTasksQueryHandler(_mockRepo.Object, _fixture.Mapper);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Tasks);
        Assert.Equal(tasks.Count, result.Tasks.Count);
    }

    [Fact]
    public async Threading.Task Handle_ReturnsTasksList_Where_State_Pending()
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
        var handler = new ListTasksQueryHandler(_mockRepo.Object, _fixture.Mapper);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Tasks);
        Assert.Equal(pendingTasks.Count(), result.Tasks.Count);
        Assert.Equal(firstPendingTask?.Id, actual: result.Tasks?.FirstOrDefault()?.Id);
    }

    [Fact]
    public async Threading.Task Handle_ReturnsTasksList_OderBy_ExpirationDate()
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
        var handler = new ListTasksQueryHandler(_mockRepo.Object, _fixture.Mapper);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Tasks);
        Assert.Equal(result.Tasks.FirstOrDefault()?.Id, actual: result.Tasks?.FirstOrDefault()?.Id);
    }

    // Add more tests here with different states and orders (also combined could be)
}