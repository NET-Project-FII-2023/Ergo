using FluentValidation;
namespace Ergo.Application.Features.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
    {
        public DeleteProjectCommandValidator()
        {
            RuleFor(p => p.ProjectId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

        }
    }
}