using Ergo.Application.Persistence;
using FluentValidation;
using MediatR;

namespace Ergo.Application.Features.TaskItems.Commands.DeleteAssignedUserFromTask
{
    public class DeleteAssignedUserFromTaskCommandHandler : IRequestHandler<DeleteAssignedUserFromTaskCommand, DeleteAssignedUserFromTaskCommandResponse>
    {
        private readonly ITaskItemRepository taskItemRepository;
        private readonly IProjectRepository projectRepository;
        public DeleteAssignedUserFromTaskCommandHandler(ITaskItemRepository taskItemRepository, IProjectRepository projectRepository)
        {
            this.taskItemRepository = taskItemRepository;
            this.projectRepository = projectRepository;
        }

        public async Task<DeleteAssignedUserFromTaskCommandResponse> Handle(DeleteAssignedUserFromTaskCommand request, CancellationToken cancellationToken)
        {
            var validator = new DeleteAssignedUserFromTaskCommandValidator();
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);
            if(!validatorResult.IsValid)
            {
                return new DeleteAssignedUserFromTaskCommandResponse()
                {
                    Success = false,
                    ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }
            var taskItem = await taskItemRepository.FindByIdAsync(request.TaskItemId);
            if(!taskItem.IsSuccess)
            {
                return new DeleteAssignedUserFromTaskCommandResponse()
                {
                    Success = false,
                    Message = "TaskItem not found"
                };
            }
            var project = await projectRepository.FindByIdAsync(taskItem.Value.ProjectId);
            if(!project.IsSuccess)
            {
                return new DeleteAssignedUserFromTaskCommandResponse()
                {
                    Success = false,
                    Message = "Project not found"
                };
            }
            if(project.Value.CreatedBy != request.Owner)
            {
                return new DeleteAssignedUserFromTaskCommandResponse()
                {
                    Success = false,
                    Message = "You are not the owner of the project!"
                };
            }
            var result = taskItem.Value.DeleteAssignedUser();
            if(!result.IsSuccess)
            {
                return new DeleteAssignedUserFromTaskCommandResponse()
                {
                    Success = false,
                    Message = result.Error
                };
            }
            var updateResult = await taskItemRepository.UpdateAsync(taskItem.Value);
            if(!updateResult.IsSuccess)
            {
                return new DeleteAssignedUserFromTaskCommandResponse()
                {
                    Success = false,
                    Message = updateResult.Error
                };
            }
            return new DeleteAssignedUserFromTaskCommandResponse { Success = true };
        }
    }
}
