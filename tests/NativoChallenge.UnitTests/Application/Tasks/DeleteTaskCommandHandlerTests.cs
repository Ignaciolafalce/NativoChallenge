using Moq;
using NativoChallenge.Application.Tasks.Commands;
using NativoChallenge.Application.Tasks.Commands.Handlers;
using NativoChallenge.Domain.Enums;
using NativoChallenge.Domain.Exceptions;
using NativoChallenge.Domain.Interfaces;
using Entities = NativoChallenge.Domain.Entities;

namespace NativoChallenge.UnitTests.Application.Tasks;

public class DeleteTaskCommandHandlerTests
{
    readonly Mock<ITaskRepository> _mockRepo = new();

    [Fact]
    public async Task Handle_DeletesTask()
    {
        // Arrange
        var task = new Entities.Task(Guid.NewGuid(), "Title", null, DateTime.UtcNow.AddDays(1), TaskPriority.Medium);
        
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(task);
        _mockRepo.Setup(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new DeleteTaskCommandHandler(_mockRepo.Object);
        var command = new DeleteTaskCommand(Guid.NewGuid());

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRepo.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeletesTask_Throw_InvalidTaskException()
    {
        try
        {
            // Arrange
            Entities.Task task = null!;
   
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(task);
            _mockRepo.Setup(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var handler = new DeleteTaskCommandHandler(_mockRepo.Object);
            var command = new DeleteTaskCommand(Guid.NewGuid());

            // Act
            await handler.Handle(command, CancellationToken.None);

        }
        catch (Exception ex)
        {
            // Assert
            _mockRepo.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            await Assert.ThrowsAnyAsync<InvalidTaskException>(() => throw ex);
        }

    }


}