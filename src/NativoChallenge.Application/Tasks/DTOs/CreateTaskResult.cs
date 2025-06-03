namespace NativoChallenge.Application.Tasks.DTOs;

public class CreateTaskResult
{
    public Guid Id { get; init; }
    public List<string> Warnings { get; init; } = new();

    public CreateTaskResult(Guid id, List<string>? warnings = null)
    {
        Id = id;
        Warnings = warnings ?? new();
    }
}