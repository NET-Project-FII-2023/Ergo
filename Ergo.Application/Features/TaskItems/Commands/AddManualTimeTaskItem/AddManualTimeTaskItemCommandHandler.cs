using Ergo.Application.Persistence;
using Ergo.Domain.Entities.Enums;
using MediatR;

namespace Ergo.Application.Features.TaskItems.Commands.AddManualTimeTaskItem
{
    public class AddManualTimeTaskItemCommandHandler : IRequestHandler<AddManualTimeTaskItemCommand, AddManualTimeTaskItemCommandResponse>
    {
        private readonly ITaskItemRepository taskItemRepository;
        public AddManualTimeTaskItemCommandHandler(ITaskItemRepository taskItemRepository)
        {
            this.taskItemRepository = taskItemRepository;
        }

        public async Task<AddManualTimeTaskItemCommandResponse> Handle(AddManualTimeTaskItemCommand request, CancellationToken cancellationToken)
        {
            var validator = new AddManualTimeTaskItemCommandValidator();
            var validatorResult = validator.Validate(request);
            if (!validatorResult.IsValid)
            {
                return new AddManualTimeTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }
            var taskItem = await taskItemRepository.FindByIdAsync(request.TaskItemId);
            if (!taskItem.IsSuccess)
            {
                return new AddManualTimeTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { taskItem.Error }
                };
            }
            if (taskItem.Value.State == TaskState.Done)
            {
                return new AddManualTimeTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Task item is already done" }
                };
            }
            var assignedUserId = await taskItemRepository.GetAssignedUser(request.TaskItemId);
            if (!assignedUserId.IsSuccess)
            {
                return new AddManualTimeTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { assignedUserId.Error }
                };
            }
            if (Guid.Parse(assignedUserId.Value) != request.UserId)
            {
                return new AddManualTimeTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Task item is not assigned to this user" }
                };
            }
            var response = taskItem.Value.AddManualTime(request.TimeSpent);
            if (!response.IsSuccess)
            {
                return new AddManualTimeTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { response.Error }
                };
            }
            var result = await taskItemRepository.UpdateAsync(taskItem.Value);
            if (!result.IsSuccess)
            {
                return new AddManualTimeTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { result.Error }
                };
            }
            return new AddManualTimeTaskItemCommandResponse
            {
                Success = true,
                TotalTimeSpent = taskItem.Value.TotalTimeSpent
            };
        }
    }
}
