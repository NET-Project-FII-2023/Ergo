using FluentValidation;

namespace Ergo.Application.Features.Photos.Commands.AddPhotoToTaskItem
{
    public class AddPhotoToTaskItemCommandValidator : AbstractValidator<AddPhotoToTaskItemCommand>
    {
        public AddPhotoToTaskItemCommandValidator()
        {
            RuleFor(p => p.TaskItemId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.Photo)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}
