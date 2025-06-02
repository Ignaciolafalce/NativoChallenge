using NativoChallenge.Application.Tasks.Commands;
using FluentValidation;

namespace NativoChallenge.Application.Tasks.Validators
{
    public class CompleteTaskValidator : AbstractValidator<CompleteTaskCommand>
    {
        public CompleteTaskValidator()
        {
            RuleFor(command => command.TaskId)
                .NotEqual(Guid.Empty).WithMessage("The task id must not be empty");
        }
    }
}
