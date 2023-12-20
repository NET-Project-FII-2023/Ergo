using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, UpdateProjectCommandResponse>
    {
        private readonly IProjectRepository projectRepository;

        public UpdateProjectCommandHandler(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        public async Task<UpdateProjectCommandResponse> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await projectRepository.FindByIdAsync(request.ProjectId);
            if (!project.IsSuccess)
            {
                return new UpdateProjectCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { project.Error }
                };
            }

            request.ProjectName ??= project.Value.ProjectName;
            request.Description ??= project.Value.Description;
            request.GitRepository ??= project.Value.GitRepository;
            if (request.Deadline == default)
            {
                request.Deadline = project.Value.Deadline;
            }
            if (request.State == default)
            {
                request.State = project.Value.State;
            }

            var validator = new UpdateProjectCommandValidator();
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validatorResult.IsValid)
            {
                return new UpdateProjectCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            var updateResult = project.Value.UpdateData(request.ProjectName, request.Description, request.GitRepository, request.Deadline, request.State, request.ModifiedBy);
            if (!updateResult.IsSuccess)
            {
                return new UpdateProjectCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { updateResult.Error }
                };
            }
            var result = await projectRepository.UpdateAsync(project.Value);

            return new UpdateProjectCommandResponse
            {
                Success = result.IsSuccess,
                Project = new UpdateProjectDto
                {
                    ProjectId = result.Value.ProjectId,
                    ProjectName = result.Value.ProjectName,
                    Description = result.Value.Description,
                    GitRepository = result.Value.GitRepository,
                    LastModifiedBy = result.Value.LastModifiedBy,
                    LastModifiedDate = result.Value.LastModifiedDate,
                    Deadline = result.Value.Deadline,
                    State = result.Value.State
                }
            };
        }
    }
}
