using Moq;
using NativoChallenge.Domain.Interfaces;
using NativoChallenge.Domain.Enums;
using NativoChallenge.Application.Tasks.Commands;
using NativoChallenge.Application.Tasks.Commands.Handlers;
using NativoChallenge.Application.Tasks.DTOs;
using Entities = NativoChallenge.Domain.Entities.Task;
using Threading = System.Threading.Tasks;

namespace NativoChallenge.UnitTests.Application.Tasks;

public class CreateTaskCommandHandlerTests
{
    readonly Mock<ITaskRepository> _mockRepo = new();

    [Fact]
    public async Task Handle_CreatesTask()
    {
        // Arrange
        var task = new Entities.Task(Guid.NewGuid(), "Title", null, DateTime.UtcNow.AddDays(1), TaskPriority.Medium);

        _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Entities.Task>(), It.IsAny<CancellationToken>())).ReturnsAsync(task);
        _mockRepo.Setup(r => r.CountHighPriorityPendingAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);


        var handler = new CreateTaskCommandHandler(_mockRepo.Object);
        var command = new CreateTaskCommand("Title", "Description", DateTime.UtcNow.AddDays(1), TaskPriority.High.ToString());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRepo.Verify(r => r.CreateAsync(It.IsAny<Entities.Task>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockRepo.Verify(r => r.CountHighPriorityPendingAsync(It.IsAny<CancellationToken>()));
        Assert.Equal(task.Id, result.Id);
    }

    [Fact]
    public async Task Handle_CreatesTask_With_HighPriorityPendingLimitExceeded_Result_Has_Warning()
    {
        // Arrange
        var task = new Entities.Task(Guid.NewGuid(), "Title", null, DateTime.UtcNow.AddDays(1), TaskPriority.Medium);

        _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Entities.Task>(), It.IsAny<CancellationToken>())).ReturnsAsync(task);
        _mockRepo.Setup(r => r.CountHighPriorityPendingAsync(It.IsAny<CancellationToken>())).ReturnsAsync(11);


        var handler = new CreateTaskCommandHandler(_mockRepo.Object);
        var command = new CreateTaskCommand("Title", "Description", DateTime.UtcNow.AddDays(1), TaskPriority.High.ToString());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRepo.Verify(r => r.CreateAsync(It.IsAny<Entities.Task>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockRepo.Verify(r => r.CountHighPriorityPendingAsync(It.IsAny<CancellationToken>()));
        Assert.IsType<CreateTaskResult>(result);

        Assert.Equal(task.Id, result.Id);
        Assert.NotEmpty(result.Warnings);
    }
}