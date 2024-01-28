using FluentValidation;

namespace Ergo.Application.Features.TaskItems.Commands.DeleteTaskItem
{
    public class DeleteTaskItemCommandValidator : AbstractValidator<DeleteTaskItemCommand>
    {
        public DeleteTaskItemCommandValidator()
        {
            RuleFor(p => p.TaskItemId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}
