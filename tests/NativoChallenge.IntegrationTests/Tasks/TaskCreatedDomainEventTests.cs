using NativoChallenge.Application.Tasks.Commands;
using NativoChallenge.Application.Tasks.DTOs;
using NativoChallenge.WebAPI.Common;
using System.Net.Http.Json;

namespace NativoChallenge.IntegrationTests.Tasks;

public class TaskCreatedDomainEventIntegrationTest : IClassFixture<TaskEndpointsSetupFixture>
{
    private readonly HttpClient _client;
    private readonly TaskEndpointsSetupFixture _fixture;

    public TaskCreatedDomainEventIntegrationTest(TaskEndpointsSetupFixture fixture)
    {
        _fixture = fixture;
        _client = fixture.CreateClient();
        _fixture.AuthenticateAdminAsync(_client).Wait(); // Ensure admin authentication for the test
    }

    [Fact]
    public async Task CreatingTask_ShouldTriggerEmailSender()
    {
        // Arrange
        var command = new CreateTaskCommand(
            Title: "Test domain event",
            Description: "Testing if event triggers email sender",
            ExpirationDate: DateTime.UtcNow.AddDays(1),
            Priority: "High"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/tasks", command);
        response.EnsureSuccessStatusCode();

        // Assert
        var responseBody = await response.Content.ReadAsStringAsync();
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<CreateTaskResult>>();

        Assert.True(FlagEmailSender.WasCalled);
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result!.Data!.Id);

        // i should add a fake implementation of IEmailSender to verify that the email was sent but no time for that :(
    }
}