using FluentValidation;

namespace Ergo.Application.Features.TaskItems.Commands.StartTimerTaskItem
{
    public class StartTimerTaskItemCommandValidator : AbstractValidator<StartTimerTaskItemCommand>
    {
        public StartTimerTaskItemCommandValidator()
        {
            RuleFor(p => p.TaskItemId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}
