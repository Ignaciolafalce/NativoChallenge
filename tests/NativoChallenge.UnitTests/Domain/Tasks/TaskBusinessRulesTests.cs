using System;
using Xunit;
using Entities = NativoChallenge.Domain.Entities.Task;
using NativoChallenge.Domain.Enums;
using NativoChallenge.Domain.Exceptions;

namespace NativoChallenge.UnitTests.Domain.Tasks;
public class TaskBusinessRulesTests
{
    [Fact]
    public void HighPriorityPendingLimitExceeded_ReturnsTrue_WhenCountGreaterThan10()
    {
        // Act
        var result = Entities.Task.HighPriorityPendingLimitExceeded(11, out var warning);

        // Assert
        Assert.True(result);
        Assert.Equal("There are more than 10 high proprity pending tasks.", warning);
    }

    [Fact]
    public void HighPriorityPendingLimitExceeded_ReturnsFalse_WhenCountIs10OrLess()
    {
        // Act
        var result = Entities.Task.HighPriorityPendingLimitExceeded(10, out var warning);

        // Assert
        Assert.False(result);
        Assert.Equal("There are more than 10 high proprity pending tasks.", warning);
    }

    [Fact]
    public void Constructor_ThrowsInvalidTaskException_WhenTitleIsEmpty()
    {
        // Act & Assert
        Assert.Throws<InvalidTaskException>(() =>
            new Entities.Task("", "desc", DateTime.UtcNow.AddDays(1), TaskPriority.Low));
    }

    [Fact]
    public void Complete_ThrowsInvalidTaskException_WhenTaskIsExpired()
    {
        // Arrange
        var task = new Entities.Task("Title", null, DateTime.UtcNow.AddDays(-1), TaskPriority.Medium);

        // Act & Assert
        Assert.Throws<InvalidTaskException>(() => task.Complete());
    }

    [Fact]
    public void Complete_SetsStateToCompleted_WhenTaskIsNotExpired()
    {
        // Arrange
        var task = new Entities.Task("Title", null, DateTime.UtcNow.AddDays(1), TaskPriority.Medium);

        // Act
        task.Complete();

        // Assert
        Assert.Equal(TaskState.Completed, task.State);
    }
}