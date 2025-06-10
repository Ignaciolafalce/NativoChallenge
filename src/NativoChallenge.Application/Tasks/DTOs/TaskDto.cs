namespace NativoChallenge.Application.Tasks.DTOs;

public class TaskDto
{
    public TaskDto()
    {
        Title = default!;
        Description = default!;
        ExpirationDate = default!;
        Priority = default!;
        Status = default!;
    }

    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime ExpirationDate { get; init; }
    public string Priority { get; init; }
    public string Status { get; init; }
}