using FluentValidation;

namespace Ergo.Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
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
                .NotEmpty().WithMessage("{Deadline} is required.")
                .NotNull();

            RuleFor(p => p.FullName)
                .NotEmpty().WithMessage("{LastModifiedBy} is required.")
                .NotNull();
        }
    }
}
