using FluentValidation;
using Entities = NativoChallenge.Domain.Entities;
using NativoChallenge.Domain.Enums;
using NativoChallenge.Application.Tasks.Queries;

namespace NativoChallenge.Application.Tasks.Validators;

public class ListTasksQueryValidator : AbstractValidator<ListTasksQuery>
{
    private static readonly string[] _allowedStateFields = [nameof(TaskState.Pending), nameof(TaskState.Completed)];

    private static readonly string[] _allowedOrderFields = [nameof(Entities.Task.Task.ExpirationDate), nameof(Entities.Task.Task.Priority)];

    public ListTasksQueryValidator()
    {
        RuleFor(query => query.State)
                .Must(field => string.IsNullOrWhiteSpace(field) || _allowedStateFields.Contains(field))
                .WithMessage($"The state field must be {string.Join(", ", _allowedStateFields)}, null or empty");

        RuleFor(query => query.OrderBy)
                .Must(field => string.IsNullOrWhiteSpace(field) || _allowedOrderFields.Contains(field))
                .WithMessage($"The orderBy field must be {string.Join(", ", _allowedOrderFields)}, null, or empty");
    }
}
