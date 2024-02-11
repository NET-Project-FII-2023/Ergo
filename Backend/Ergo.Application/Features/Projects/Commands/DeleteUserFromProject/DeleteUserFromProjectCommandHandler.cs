using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.Projects.Commands.DeleteUserFromProject
{
    public class DeleteUserFromProjectCommandHandler : IRequestHandler<DeleteUserFromProjectCommand, DeleteUserFromProjectCommandResponse>
    {
        private readonly IProjectRepository projectRepository;
        private readonly IUserManager userManager;

        public DeleteUserFromProjectCommandHandler(IProjectRepository projectRepository, IUserManager userManager)
        {
            this.projectRepository = projectRepository;
            this.userManager = userManager;
        }

        public async Task<DeleteUserFromProjectCommandResponse> Handle(DeleteUserFromProjectCommand request, CancellationToken cancellationToken)
            {
            var project = await projectRepository.FindByIdAsync(Guid.Parse(request.ProjectId));
            if (!project.IsSuccess)
            {
                return new DeleteUserFromProjectCommandResponse
                {
                    Success = false,
                    Message = "Project not found."
                };
            }
            var user = await userManager.FindByIdAsync(Guid.Parse(request.UserId));
            if (!user.IsSuccess)
            {
                return new DeleteUserFromProjectCommandResponse
                {
                    Success = false,
                    Message = "User not found."
                };
            }
            var owner = await userManager.FindByUsernameAsync(request.OwnerUsername);
            if (!owner.IsSuccess)
            {
                return new DeleteUserFromProjectCommandResponse
                {
                    Success = false,
                    Message = "Owner not found."
                };
            }

            if (project.Value.CreatedBy != request.OwnerUsername)
            {
                return new DeleteUserFromProjectCommandResponse
                {
                    Success = false,
                    Message = "You are not the owner of this project."
                };
            }
            if (project.Value.CreatedBy == user.Value.Username)
            {
                return new DeleteUserFromProjectCommandResponse
                {
                    Success = false,
                    Message = "You cannot delete the owner of the project."
                };
            }

            var result = await projectRepository.DeleteUserFromProjectAsync(Guid.Parse(request.ProjectId), Guid.Parse(request.UserId));
            if (!result)
            {
                return new DeleteUserFromProjectCommandResponse
                {
                    Success = false,
                    Message = "Failed to delete user from project."
                };
            }
            return new DeleteUserFromProjectCommandResponse
            {
                Success = true,
                Message = "User deleted from project successfully."
            }; 
        }
    }
}
