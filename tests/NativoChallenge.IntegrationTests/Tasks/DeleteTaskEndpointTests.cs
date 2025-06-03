using NativoChallenge.Application.Tasks.Commands;
using System.Net;
using System.Net.Http.Json;
using NativoChallenge.Domain.Enums;
using NativoChallenge.Application.Tasks.DTOs;

namespace NativoChallenge.IntegrationTests.Tasks;

public class DeleteTaskEndpointTests : IClassFixture<TaskEndpointsSetupFixture>
{
    private readonly TaskEndpointsSetupFixture _fixture;
    private readonly HttpClient _client;

    public DeleteTaskEndpointTests(TaskEndpointsSetupFixture fixture)
    {
        _fixture = fixture;
        _client = fixture.CreateClient();
    }

    [Fact]
    public async Task DeleteTask_DeletesTaskAndReturnsNoContent()
    {
        // Arrange
        var createCommand = new CreateTaskCommand(
            Title: "Task to Complete",
            Description: "To be completed",
            ExpirationDate: DateTime.UtcNow.AddDays(1),
            Priority: TaskPriority.High.ToString()
        );

        // Act
        var createResponse = await _client.PostAsJsonAsync("/tasks", createCommand);
        var createResult = await createResponse.Content.ReadFromJsonAsync<NativoChallenge.WebAPI.Common.ApiResponse<CreateTaskResult>>();
        var taskId = createResult!.Data.Id;
        var response = await _client.DeleteAsync($"/tasks/{taskId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}