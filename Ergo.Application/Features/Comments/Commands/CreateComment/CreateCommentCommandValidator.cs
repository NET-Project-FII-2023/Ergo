using FluentValidation;


namespace Ergo.Application.Features.Comments.Commands.CreateComment
{
    public class CreateCommentCommandValidator: AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(p => p.CommentText)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(500).WithMessage("{PropertyName} must not exceed 500 characters.");
            RuleFor(p => p.CreatedBy)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();
            
            
        }
    }
}
