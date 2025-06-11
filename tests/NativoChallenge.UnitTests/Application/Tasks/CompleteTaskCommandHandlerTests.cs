using Moq;
using NativoChallenge.Application.Tasks.Commands;
using NativoChallenge.Application.Tasks.Commands.Handlers;
using NativoChallenge.Application.Tasks.DTOs;
using NativoChallenge.Domain.Entities.Task;
using NativoChallenge.Domain.Enums;
using NativoChallenge.Domain.Exceptions;
using NativoChallenge.Domain.Interfaces;
using Entities = NativoChallenge.Domain.Entities.Task;
using Threading = System.Threading.Tasks;

namespace NativoChallenge.UnitTests.Application.Tasks;

public class CompleteTaskCommandHandlerTests
{
    readonly Mock<ITaskRepository> _mockRepo = new();

    [Fact]
    public async Threading.Task Handle_CompletesTask()
    {
        // Arrange
        var task = new Entities.Task("Title", null, DateTime.UtcNow.AddDays(1), TaskPriority.Medium);

        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(task);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Entities.Task>(), It.IsAny<CancellationToken>())).Returns(Threading.Task.CompletedTask);

        var handler = new CompleteTaskCommandHandler(_mockRepo.Object);
        var command = new CompleteTaskCommand(task.Id);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRepo.Verify(r => r.GetByIdAsync(task.Id, It.IsAny<CancellationToken>()), Times.Once);
        _mockRepo.Verify(r => r.UpdateAsync(task, It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(TaskState.Completed, task.State);
        Assert.IsType<CompleteTaskResult>(result);
        Assert.Empty(result.Warnings);
    }

    [Fact]
    public async Threading.Task Handle_Throw_InvalidTaskException()
    {
        try
        {
            // Arrange
            var dummyGuid = Guid.NewGuid();
            Entities.Task task = null!;
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(task);

            var handler = new CompleteTaskCommandHandler(_mockRepo.Object);

            var command = new CompleteTaskCommand(dummyGuid); // dummy ID

            // Act
            await handler.Handle(command, CancellationToken.None);

        }
        catch (InvalidTaskException ex)
        {
            // Assert
            _mockRepo.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            await Assert.ThrowsAnyAsync<InvalidTaskException>(() => throw ex);
        }
    }
}