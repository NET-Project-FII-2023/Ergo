using FluentValidation;

namespace Ergo.Application.Features.Projects.Queries.GetProjectGithubData
{
    public class GetProjectGithubDataQueryValidator : AbstractValidator<GetProjectGithubDataQuery>
    {
        public GetProjectGithubDataQueryValidator()
        {
            RuleFor(p => p.ProjectId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}
