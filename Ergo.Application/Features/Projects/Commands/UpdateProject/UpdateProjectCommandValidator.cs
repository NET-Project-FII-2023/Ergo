using FluentValidation;
namespace Ergo.Application.Features.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
        {
            {
                RuleFor(p => p.ProjectName)
                    .NotEmpty().WithMessage("{PropertyName} is required.")
                    .NotNull()
                    .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

                RuleFor(p => p.Description)
                    .NotEmpty().WithMessage("{PropertyName} is required.")
                    .NotNull()
                    .MaximumLength(5000).WithMessage("{PropertyName} must not exceed 5000 characters.");

                RuleFor(p => p.Deadline)
                    .NotEmpty().WithMessage("{PropertyName} is required.")
                    .NotNull();

                RuleFor(p => p.ModifiedBy)
                    .NotEmpty().WithMessage("{PropertyName} is required.")
                    .NotNull();

                RuleFor(p => p.State)
                    .IsInEnum()
                    .WithMessage("{PropertyName} must be a valid value between 0-4");
            }
        }
    }
}
