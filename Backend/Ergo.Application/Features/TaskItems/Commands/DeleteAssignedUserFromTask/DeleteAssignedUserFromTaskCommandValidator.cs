using FluentValidation;

namespace Ergo.Application.Features.TaskItems.Commands.DeleteAssignedUserFromTask
{
    public class DeleteAssignedUserFromTaskCommandValidator : AbstractValidator<DeleteAssignedUserFromTaskCommand>
    {
        public DeleteAssignedUserFromTaskCommandValidator()
        {
            RuleFor(p => p.TaskItemId)
                .NotEmpty().WithMessage("TaskItemId is required.")
                .NotNull();
            RuleFor(p => p.Owner)
                .NotEmpty().WithMessage("UserIdOwner is required.")
                .NotNull();
        }
    }
}
