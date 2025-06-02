namespace NativoChallenge.WebAPI.Common;

public class ApiResponse<T>
{
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public static ApiResponse<T> Success(T data) =>
        new() { Data = data };

    public static ApiResponse<T> Failure(List<string> errors) =>
        new() { Errors = errors };
}
