using Ergo.Application.Features.TaskItems.Commands.AssignTaskItemToUser;
using Ergo.Application.Persistence;
using Ergo.Domain.Entities.Enums;
using MediatR;

namespace Ergo.Application.Features.TaskItems.Commands.StartTimerTaskItem
{
    public class StartTimerTaskItemCommandHandler : IRequestHandler<StartTimerTaskItemCommand, StartTimerTaskItemCommandResponse>
    {
        private readonly ITaskItemRepository taskItemRepository;

        public StartTimerTaskItemCommandHandler(ITaskItemRepository taskItemRepository)
        {
            this.taskItemRepository = taskItemRepository;
        }
        public async Task<StartTimerTaskItemCommandResponse> Handle(StartTimerTaskItemCommand request, CancellationToken cancellationToken)
        {
            var validator = new StartTimerTaskItemCommandValidator();
            var validatorResult = validator.Validate(request);
            if(!validatorResult.IsValid)
            {
                return new StartTimerTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }
            var taskItem = await taskItemRepository.FindByIdAsync(request.TaskItemId);
            if (!taskItem.IsSuccess)
            {
                return new StartTimerTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { taskItem.Error }
                }; 
            }
            if (taskItem.Value.State == TaskState.Done)
            {
                return new StartTimerTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Task item is already done" }
                };
            }
            var assignedUserId = await taskItemRepository.GetAssignedUser(request.TaskItemId);
            if(!assignedUserId.IsSuccess)
            {
                return new StartTimerTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { assignedUserId.Error }
                };
            }
            if(Guid.Parse(assignedUserId.Value) != request.UserId)
            {
                return new StartTimerTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Task item is not assigned to this user" }
                };
            }
            
            var response = taskItem.Value.StartOrResumeTask();
            if (!response.IsSuccess)
            {
                return new StartTimerTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { response.Error }
                };
            }
            var result = await taskItemRepository.UpdateAsync(taskItem.Value);
            return new StartTimerTaskItemCommandResponse
            {
                Success = result.IsSuccess,
                StartTime = taskItem.Value.StartTime.Value
            };
        }
    }
}
