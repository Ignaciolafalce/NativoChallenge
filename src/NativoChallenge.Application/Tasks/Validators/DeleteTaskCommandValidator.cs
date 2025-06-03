using NativoChallenge.Application.Tasks.Commands;
using FluentValidation;

namespace NativoChallenge.Application.Tasks.Validators;

public class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommand>
{
    public DeleteTaskCommandValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEqual(Guid.Empty).WithMessage("The task id must not be empty");
            //.Must(field => ValidatorHelpers.isGuidType(field)).WithMessage("The task id must not be empty");
    }
}
