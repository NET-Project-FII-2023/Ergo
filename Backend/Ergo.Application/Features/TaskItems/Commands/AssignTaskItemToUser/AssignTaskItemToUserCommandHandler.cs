using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.TaskItems.Commands.AssignTaskItemToUser
{
    public class AssignTaskItemToUserCommandHandler : IRequestHandler<AssignTaskItemToUserCommand, AssignTaskItemToUserCommandResponse>
    {
        private readonly ITaskItemRepository taskItemRepository;


        private readonly IUserRepository userRepository;
        public AssignTaskItemToUserCommandHandler(ITaskItemRepository taskItemRepository, IUserRepository userRepository)
        {
            this.taskItemRepository = taskItemRepository;
            this.userRepository = userRepository;
        }

        public async Task<AssignTaskItemToUserCommandResponse> Handle(AssignTaskItemToUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = new AssignTaskItemToUserCommandValidator().Validate(request);
            if (!validationResult.IsValid)
            {
                return new AssignTaskItemToUserCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList()
                };
            }
            var taskItem = await taskItemRepository.FindByIdAsync(request.TaskItemId);
            if (!taskItem.IsSuccess)
            {
                return new AssignTaskItemToUserCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { taskItem.Error }
                };
            }
            var user = await userRepository.FindByIdAsync(request.UserId);
            if (!user.IsSuccess)
            {
                return new AssignTaskItemToUserCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { user.Error }
                };
            }
            var assignResult = taskItem.Value.AssignUser(user.Value);
            if (!assignResult.IsSuccess)
            {
                return new AssignTaskItemToUserCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { assignResult.Error }
                };
            }
            var result = await taskItemRepository.UpdateAsync(taskItem.Value);
            return new AssignTaskItemToUserCommandResponse
            {
                Success = result.IsSuccess
            };
        }
    }
}
