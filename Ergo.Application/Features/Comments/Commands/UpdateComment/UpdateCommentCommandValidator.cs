using FluentValidation;


namespace Ergo.Application.Features.Comments.Commands.UpdateComment
{
    public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentCommandValidator()
        {
            RuleFor(p => p.CommentText)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(500).WithMessage("{PropertyName} must not exceed 500 characters.");
            RuleFor(p => p.Task)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();
            RuleFor(p => p.CreatedBy)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();



        }
    }
}
