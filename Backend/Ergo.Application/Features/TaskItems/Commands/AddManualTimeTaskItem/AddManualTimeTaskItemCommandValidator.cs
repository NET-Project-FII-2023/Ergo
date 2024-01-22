using FluentValidation;

namespace Ergo.Application.Features.TaskItems.Commands.AddManualTimeTaskItem
{
    public class AddManualTimeTaskItemCommandValidator : AbstractValidator<AddManualTimeTaskItemCommand>
    {
        public AddManualTimeTaskItemCommandValidator()
        {
            RuleFor(p => p.TaskItemId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.TimeSpent)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}
