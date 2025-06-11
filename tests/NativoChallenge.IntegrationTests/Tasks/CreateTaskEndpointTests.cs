using NativoChallenge.Application.Tasks.Commands;
using NativoChallenge.Application.Tasks.DTOs;
using NativoChallenge.Domain.Enums;
using NativoChallenge.WebAPI.Common;
using System.Net;
using System.Net.Http.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace NativoChallenge.IntegrationTests.Tasks;

public class CreateTaskEndpointTests : IClassFixture<TaskEndpointsSetupFixture>
{
    private readonly TaskEndpointsSetupFixture _fixture;
    private readonly HttpClient _client;

    public CreateTaskEndpointTests(TaskEndpointsSetupFixture fixture)
    {
        _fixture = fixture;
        _client = fixture.CreateClient();
        _fixture.AuthenticateAdminAsync(_client).Wait();
    }

    [Fact]
    public async Task CreateTask_CompletesTaskAndReturnsNoContent()
    {
        // Arrange
        var createCommand = new CreateTaskCommand(
            Title: "Task to Complete",
            Description: "To be completed",
            ExpirationDate: DateTime.UtcNow.AddDays(1),
            Priority: TaskPriority.High.ToString()
        );


        // Act
        var response = await _client.PostAsJsonAsync("/tasks", createCommand);
        response.EnsureSuccessStatusCode();
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<CreateTaskResult>>();

        // Assert
        Assert.NotNull(apiResponse);
        Assert.Null(apiResponse.Errors);
        Assert.NotNull(apiResponse.Data);
        Assert.NotEqual(Guid.Empty, apiResponse.Data.Id);
    }

    // the end (not really we should are more scenarios)...
}