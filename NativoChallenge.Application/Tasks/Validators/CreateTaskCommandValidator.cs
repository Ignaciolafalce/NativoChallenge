using NativoChallenge.Application.Tasks.Commands;
using NativoChallenge.Domain.Enums;
using FluentValidation;

namespace NativoChallenge.Application.Tasks.Validators
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        private static readonly string[] _allowedPriorities = Enum.GetNames(typeof(TaskPriority));
        public CreateTaskCommandValidator()
        {
            RuleFor(command => command.Title).NotEmpty().WithMessage("The title must not be empty");

            RuleFor(command => command.Description).NotEmpty().WithMessage("The description must not be empty");

            RuleFor(command => command.ExpirationDate)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("The expiration date must be grater than today");

            RuleFor(command => command.Priority)
                .Must(command => Enum.GetNames(typeof(TaskPriority)).Contains(command))
                .WithMessage($"The priority must be one of de follow: {string.Join(" | ", _allowedPriorities)}");

        }
    }

}
