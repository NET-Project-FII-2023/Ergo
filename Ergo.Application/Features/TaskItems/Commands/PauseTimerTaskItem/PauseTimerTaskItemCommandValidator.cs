using FluentValidation;

namespace Ergo.Application.Features.TaskItems.Commands.PauseTimerTaskItem
{
    public class PauseTimerTaskItemCommandValidator : AbstractValidator<PauseTimerTaskItemCommand>
    {
        public PauseTimerTaskItemCommandValidator()
        {
            RuleFor(x => x.TaskItemId).NotEmpty().WithMessage("Task item id is required");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User id is required");
        }
    }
}
