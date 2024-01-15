using Ergo.Application.Features.TaskItems.Commands.StartTimerTaskItem;
using Ergo.Application.Persistence;
using Ergo.Domain.Entities.Enums;
using MediatR;

namespace Ergo.Application.Features.TaskItems.Commands.PauseTimerTaskItem
{
    public class PauseTimerTaskItemCommandHandler : IRequestHandler<PauseTimerTaskItemCommand, PauseTimerTaskItemCommandResponse>
    {
        private readonly ITaskItemRepository taskItemRepository;
        public PauseTimerTaskItemCommandHandler(ITaskItemRepository taskItemRepository)
        {
            this.taskItemRepository = taskItemRepository;
        }

        public async Task<PauseTimerTaskItemCommandResponse> Handle(PauseTimerTaskItemCommand request, CancellationToken cancellationToken)
        {
            var validator = new PauseTimerTaskItemCommandValidator();
            var validatorResult = validator.Validate(request);
            if (!validatorResult.IsValid)
            {
                return new PauseTimerTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }
            var taskItem = await taskItemRepository.FindByIdAsync(request.TaskItemId);
            if (!taskItem.IsSuccess)
            {
                return new PauseTimerTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { taskItem.Error }
                };
            }
            if (taskItem.Value.State == TaskState.Done)
            {
                return new PauseTimerTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Task item is already done" }
                };
            }
            var assignedUserId = await taskItemRepository.GetAssignedUser(request.TaskItemId);
            if (!assignedUserId.IsSuccess)
            {
                return new PauseTimerTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { assignedUserId.Error }
                };
            }
            if (Guid.Parse(assignedUserId.Value) != request.UserId)
            {
                return new PauseTimerTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Task item is not assigned to this user" }
                };
            }
            var respone = taskItem.Value.PauseTask();
            if (!respone.IsSuccess)
            {
                return new PauseTimerTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { respone.Error }
                };
            }
            var result = await taskItemRepository.UpdateAsync(taskItem.Value);
            if (!result.IsSuccess)
            {
                return new PauseTimerTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { result.Error }
                };
            } 
            return new PauseTimerTaskItemCommandResponse
            {
                Success = true,
                TotalTimeSpent = taskItem.Value.TotalTimeSpent
            };

        }
    }

}
