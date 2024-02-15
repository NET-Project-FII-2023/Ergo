using Ergo.Application.Persistence;
using Ergo.Domain.Entities;
using MediatR;

namespace Ergo.Application.Features.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommandHandler :IRequestHandler<DeleteProjectCommand, DeleteProjectCommandResponse>
    {
        private readonly IProjectRepository repository;
        private readonly ITaskItemRepository taskItemRepository;
        private readonly ICommentRepository commentRepository;

        public DeleteProjectCommandHandler(IProjectRepository repository, ITaskItemRepository taskItemRepository, ICommentRepository commentRepository)
        {
            this.repository = repository;
            this.taskItemRepository = taskItemRepository;
            this.commentRepository = commentRepository;
        }

        public async Task<DeleteProjectCommandResponse> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            DeleteProjectCommandResponse response = new();
            var validator = new DeleteProjectCommandValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.ValidationsErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return response;
            }
            var projectToDelete = await repository.FindByIdAsync(request.ProjectId);

            if (projectToDelete == null)
            {
                response.Success = false;
                response.ValidationsErrors = new List<string> { "Project not found" };
                return response;
            }
            if(projectToDelete.Value.CreatedBy != request.Owner)
            {
                response.Success = false;
                response.ValidationsErrors = new List<string> { "You are not the owner of this project" };
                return response;
            }
            var tasksProject = await taskItemRepository.GetTasksByProjectIdAsync(request.ProjectId);
            if (!tasksProject.IsSuccess)
            {
                response.Success = false;
                response.ValidationsErrors = new List<string> { tasksProject.Error };
                return response;
            }
            List<Comment> commentsProject = new List<Comment>();
            foreach(var task in tasksProject.Value)
            {
                var comments = await commentRepository.GetCommentByTaskIdAsync(task.TaskItemId);
                if (!comments.IsSuccess)
                {
                    response.Success = false;
                    response.ValidationsErrors = new List<string> { comments.Error };
                    return response;
                }
                commentsProject.AddRange(comments.Value);            
            }
            var result = await repository.DeleteAsync(request.ProjectId);
            if (!result.IsSuccess)
            {
                response.Success = false;
                response.ValidationsErrors = new List<string> { result.Error };
                return response;
            }
            foreach (var task in tasksProject.Value)
            {
                var taskResult = await taskItemRepository.DeleteAsync(task.TaskItemId);
                if (!taskResult.IsSuccess)
                {
                    response.Success = false;
                    response.ValidationsErrors = new List<string> { taskResult.Error };
                    return response;
                }
            }
            foreach (var comment in commentsProject)
            {
                var commentResult = await commentRepository.DeleteAsync(comment.CommentId);
                if (!commentResult.IsSuccess)
                {
                    response.Success = false;
                    response.ValidationsErrors = new List<string> { commentResult.Error };
                    return response;
                }
            }
            return new DeleteProjectCommandResponse
            {
                Success = true
            };

        }
    }
    
}
