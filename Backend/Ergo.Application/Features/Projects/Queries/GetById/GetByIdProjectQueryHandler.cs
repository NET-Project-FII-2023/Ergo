using Ergo.Application.Persistence;
using MediatR;
namespace Ergo.Application.Features.Projects.Queries.GetById
{
    public class GetByIdProjectQueryHandler : IRequestHandler<GetByIdProjectQuery, ProjectDto>
    {
        private readonly IProjectRepository projectRepository;

        public GetByIdProjectQueryHandler(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        public async Task<ProjectDto> Handle(GetByIdProjectQuery request, CancellationToken cancellationToken)
        {
            var project = await projectRepository.FindByIdAsync(request.ProjectId);

            if (project.IsSuccess)
            {
                return new ProjectDto
                {
                    ProjectId = project.Value.ProjectId,
                    ProjectName = project.Value.ProjectName,
                    Description = project.Value.Description,
                    GitRepository = project.Value.GitRepository,
                    StartDate = project.Value.StartDate,
                    Deadline = project.Value.Deadline,
                    State = project.Value.State,
                    Members = project.Value.Members,
                    CreatedBy = project.Value.CreatedBy,
                    GithubToken = project.Value.GithubToken,
                    GithubOwner = project.Value.GithubOwner
                };
            }

            return new ProjectDto();
        }
    }
}

