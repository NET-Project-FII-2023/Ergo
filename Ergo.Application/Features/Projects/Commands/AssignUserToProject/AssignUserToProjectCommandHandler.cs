using Ergo.Application.Features.Projects.Commands.UpdateProject;
using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.Projects.Commands.AssignUserToProject
{
    public class AssignUserToProjectCommandHandler : IRequestHandler<AssignUserToProjectCommand, AssignUserToProjectCommandResponse>
    {
        private readonly IProjectRepository projectRepository;
        private readonly IUserRepository userRepository;

        public AssignUserToProjectCommandHandler(IProjectRepository projectRepository, IUserRepository userRepository)
        {
            this.projectRepository = projectRepository;
            this.userRepository = userRepository;
        }
        public async Task<AssignUserToProjectCommandResponse> Handle(AssignUserToProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await projectRepository.FindByIdAsync(Guid.Parse(request.ProjectId));
            if (!project.IsSuccess)
            {
                return new AssignUserToProjectCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { project.Error }
                };
            }
            var user = await userRepository.FindByIdAsync(Guid.Parse(request.UserId));
            if (!user.IsSuccess)
            {
                return new AssignUserToProjectCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { user.Error }
                };
            }
            project.Value.AssignUser(user.Value);
            var result = await projectRepository.UpdateAsync(project.Value);
            return new AssignUserToProjectCommandResponse
            {
                Success = result.IsSuccess
            };
        }
    }
}
