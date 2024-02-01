using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.Projects.Queries.GetProjectGithubData
{
    public class GetProjectGithubDataHandler : IRequestHandler<GetProjectGithubDataQuery, GetProjectGithubDataQueryResponse>
    {
        private readonly IProjectRepository projectRepository;

        public GetProjectGithubDataHandler(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        public async Task<GetProjectGithubDataQueryResponse> Handle(GetProjectGithubDataQuery request, CancellationToken cancellationToken)
        {
            var validator = new GetProjectGithubDataQueryValidator();
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validatorResult.IsValid)
            {
                return new GetProjectGithubDataQueryResponse
                {
                    Success = false,
                    ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }
            var project = await projectRepository.FindByIdAsync(request.ProjectId);
            if (!project.IsSuccess)
            {
                return new GetProjectGithubDataQueryResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Project with this id doesn't exist!" }
                };
            }
            if(project.Value.GithubOwner == null || project.Value.GitRepository == null)
            {
                return new GetProjectGithubDataQueryResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Project does not have github data" }
                };
            }
            return new GetProjectGithubDataQueryResponse
            {
                Success = true,
                ProjectOwner = project.Value.GithubOwner,
                ProjectRepository = project.Value.GitRepository,
                GithubToken = project.Value.GithubToken
            };
        }
    }
}
