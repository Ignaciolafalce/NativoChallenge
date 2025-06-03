using NativoChallenge.Application.Tasks.DTOs;
using NativoChallenge.WebAPI.Common;
using System.Net.Http.Json;

namespace NativoChallenge.IntegrationTests.Tasks;

public class ListTasksEndpointTests : IClassFixture<TaskEndpointsSetupFixture>
{
    private readonly TaskEndpointsSetupFixture _fixture;
    private readonly HttpClient _client;

    public ListTasksEndpointTests(TaskEndpointsSetupFixture fixture)
    {
        _fixture = fixture;
        _client = fixture.CreateClient();
    }

    [Fact]
    public async Task GetTasks_DefaultOrder_ReturnsOkAndListTasksResult()
    {
        // Arrange
        var tasksInDb = await _fixture.DefaultSeedAsync();
        tasksInDb = tasksInDb.OrderBy(t => t.Id).ToList();

        // Act
        var response = await _client.GetAsync("/tasks");
        response.EnsureSuccessStatusCode();
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<ListTasksResult>>();

        // Assert
        Assert.NotNull(apiResponse);
        Assert.IsType<ApiResponse<ListTasksResult>>(apiResponse);
        Assert.Null(apiResponse.Errors);
        Assert.NotNull(apiResponse.Data);
        Assert.NotNull(apiResponse.Data.Tasks);
        Assert.Equal(tasksInDb.Count, apiResponse.Data.Tasks.Count);
        Assert.Equal(tasksInDb.Select(t => t.Id).ToList(), apiResponse.Data.Tasks.Select(t => t.Id).ToList());
    }

    // More tests can be added here to cover different scenarios, such as filtering, sorting, warnings, etc.

}