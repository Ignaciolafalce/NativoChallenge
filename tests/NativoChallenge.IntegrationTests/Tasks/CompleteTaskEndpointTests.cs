using NativoChallenge.Application.Tasks.Commands;
using NativoChallenge.Application.Tasks.DTOs;
using NativoChallenge.Domain.Enums;
using NativoChallenge.WebAPI.Common;
using System.Net;
using System.Net.Http.Json;

namespace NativoChallenge.IntegrationTests.Tasks;

public class CompleteTaskEndpointTests : IClassFixture<TaskEndpointsSetupFixture>
{
    private readonly TaskEndpointsSetupFixture _fixture;
    private readonly HttpClient _client;

    public CompleteTaskEndpointTests(TaskEndpointsSetupFixture fixture)
    {
        _fixture = fixture;
        _client = fixture.CreateClient();
        _fixture.AuthenticateAdminAsync(_client).Wait();
    }

    [Fact]
    public async Task CompleteTask_CompletesTaskAndReturnsNoContent()
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
        var createResult = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateTaskResult>>();
        var taskId = createResult!.Data.Id;
        var response = await _client.PutAsync($"/tasks/{taskId}/complete", null);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    // More and more tests here ... i just want to finish :)
}