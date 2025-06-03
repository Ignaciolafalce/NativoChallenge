namespace NativoChallenge.Application.Tasks.DTOs;

public record TaskDto(
    Guid Id,
    string Title,
    string Description,
    DateTime ExpirationDate,
    string Priority,
    string Status
);