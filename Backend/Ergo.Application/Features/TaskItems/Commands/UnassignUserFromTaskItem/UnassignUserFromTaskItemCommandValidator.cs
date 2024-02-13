using FluentValidation;

namespace Ergo.Application.Features.TaskItems.Commands.UnassignUserFromTaskItem
{
    public class UnassignUserFromTaskItemCommandValidator : AbstractValidator<UnassignUserFromTaskItemCommand>
    {
        public UnassignUserFromTaskItemCommandValidator()
        {
            RuleFor(p => p.TaskItemId)
                .NotEmpty().WithMessage("TaskItemId is required.");

            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(p => p.OwnerUsername)
                .NotEmpty().WithMessage("OwnerUsername is required.");
        }
    }
}
