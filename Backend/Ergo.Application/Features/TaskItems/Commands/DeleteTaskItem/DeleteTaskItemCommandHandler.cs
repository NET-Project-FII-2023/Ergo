using Ergo.Application.Features.Projects.Commands.DeleteUserFromProject;
using Ergo.Application.Features.TaskItems.Commands.AssignTaskItemToUser;
using Ergo.Application.Persistence;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ergo.Application.Features.TaskItems.Commands.DeleteTaskItem
{
    public class DeleteTaskItemCommandHandler : IRequestHandler<DeleteTaskItemCommand, DeleteTaskItemCommandResponse>
    {
        private readonly ITaskItemRepository taskRepository;
        private readonly IUserManager userManager;
        private readonly IProjectRepository projectRepository;
        private readonly ICommentRepository commentRepository;



        public DeleteTaskItemCommandHandler(ITaskItemRepository repository, IUserManager userManager, IProjectRepository projectRepository, ICommentRepository commentRepository)
        {
            this.taskRepository = repository;
            this.userManager = userManager;
            this.projectRepository = projectRepository;
            this.commentRepository = commentRepository;
        }

        public async Task<DeleteTaskItemCommandResponse> Handle(DeleteTaskItemCommand request, CancellationToken cancellationToken)
        {
            DeleteTaskItemCommandResponse response = new();
            var validator = new DeleteTaskItemCommandValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.ValidationsErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return response;
            }

            var taskItemToDelete = await taskRepository.FindByIdAsync(request.TaskItemId);

            if (!taskItemToDelete.IsSuccess)
            {
                response.Success = false;
                response.ValidationsErrors = new List<string> { "TaskItem not found" };
                return response;
            }
            var project = await projectRepository.FindByIdAsync(taskItemToDelete.Value.ProjectId);
            if (!project.IsSuccess)
            {
                response.Success = false;
                response.ValidationsErrors = new List<string> { "Project not found" };
                return response;
            }
            var user = await userManager.FindByIdAsync(request.UserId);
            if (!user.IsSuccess)
            {
                response.Success = false;
                response.ValidationsErrors = new List<string> { "User not found" };
                return response;
            }
            if(project.Value.CreatedBy != user.Value.Username)
            {
                response.Success = false;
                response.ValidationsErrors = new List<string> { "You are not the owner of this project." };
                return response;
            }
            var comments = await commentRepository.GetCommentByTaskIdAsync(request.TaskItemId);
            if (!comments.IsSuccess)
            {
                response.Success = false;
                response.ValidationsErrors = new List<string> { comments.Error };
                return response;
            }


            var result = await taskRepository.DeleteAsync(request.TaskItemId);

            if (!result.IsSuccess)
            {
                response.Success = false;
                response.ValidationsErrors = new List<string> { result.Error };
                return response;
            }
            foreach (var comment in comments.Value)
            {
                var commentResult = await commentRepository.DeleteAsync(comment.CommentId);
                if (!result.IsSuccess)
                {
                    response.Success = false;
                    response.ValidationsErrors = new List<string> { result.Error };
                    return response;
                }
            }

            return new DeleteTaskItemCommandResponse
            {
                Success = true
            };
        }
    }
}
