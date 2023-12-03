using Ergo.Application.Persistence;
using MediatR;
namespace Ergo.Application.Features.Projects.Queries.GetAll
{
    public class GetAllProjectsHandler : IRequestHandler<GetAllProjectsQuery, GetAllProjectsResponse>
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
                    ProjectId = project.ProjectId,
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
