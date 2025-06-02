namespace NativoChallenge.Application.Tasks.DTOs;
public class ListTasksResult
{
    public List<TaskDto> Tasks { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
    public ListTasksResult(List<TaskDto> tasks, List<string> warnings)
    {
        Tasks = tasks ?? new List<TaskDto>();
        Warnings = warnings ?? new List<string>();
    }
}
