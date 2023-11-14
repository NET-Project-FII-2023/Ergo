using Ergo.Application.Persistence;

namespace Ergo.Application.Features.Projects.Queries.GetAll
{
    public class GetAllProjectsHandler
    {
        private readonly IProjectRepository projectRepository;

        public GetAllProjectsHandler(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        public async Task<GetAllProjectsResponse> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var response = new GetAllProjectsResponse();
            var result = await projectRepository.GetAllAsync();

            if (result.IsSuccess)
            {
                response.Projects = result.Value.Select(project => new ProjectDto
                {
                    ProjectName = project.ProjectName,
                    Description = project.Description,
                    StartDate = project.StartDate,
                    Deadline = project.Deadline,
                    State = project.State,
                    Members = project.Members
                }).ToList();
            }

            return response;
        }
    }
}
