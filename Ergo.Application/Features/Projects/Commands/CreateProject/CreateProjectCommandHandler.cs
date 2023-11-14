using Ergo.Application.Persistence;
using Ergo.Domain.Entities;
using MediatR;
namespace Ergo.Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, CreateProjectCommandResponse>
    {
        private readonly IProjectRepository projectRepository;

        public CreateProjectCommandHandler(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        public async Task<CreateProjectCommandResponse> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateProjectCommandValidator();
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validatorResult.IsValid)
            {
                return new CreateProjectCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            var project = Project.Create(request.ProjectName, request.Description, request.Deadline, request.FullName);
            if (!project.IsSuccess)
            {
                return new CreateProjectCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { project.Error }
                };
            }

            await projectRepository.AddAsync(project.Value);
            return new CreateProjectCommandResponse
            {
                Success = true,
                Project = new CreateProjectDto()
                {
                    ProjectId = project.Value.ProjectId,
                    ProjectName = project.Value.ProjectName,
                    Deadline = project.Value.Deadline,
                    Description = project.Value.Description,
                    FullName = project.Value.CreatedBy
                }
            };
        }
    }
}
