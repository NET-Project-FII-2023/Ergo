using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.TaskItems.Commands.UnassignUserFromTaskItem
{
    public class UnassignUserFromTaskItemCommandHandler : IRequestHandler<UnassignUserFromTaskItemCommand, UnassignUserFromTaskItemCommandResponse>
    {
        private readonly ITaskItemRepository taskItemRepository;
        private readonly IUserRepository userRepository;
        private readonly IProjectRepository projectRepository;
        public UnassignUserFromTaskItemCommandHandler(ITaskItemRepository taskItemRepository, IUserRepository userRepository, IProjectRepository projectRepository)
        {
            this.taskItemRepository = taskItemRepository;
            this.userRepository = userRepository;
            this.projectRepository = projectRepository;
        }

        public async Task<UnassignUserFromTaskItemCommandResponse> Handle(UnassignUserFromTaskItemCommand request, CancellationToken cancellationToken)
        {
            var validationResult = new UnassignUserFromTaskItemCommandValidator().Validate(request);
            if (!validationResult.IsValid)
            {
                return new UnassignUserFromTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList()
                };
            }
            var taskItem = await taskItemRepository.FindByIdAsync(request.TaskItemId);
            if (!taskItem.IsSuccess)
            {
                return new UnassignUserFromTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { taskItem.Error }
                };
            }
            var user = await userRepository.FindByIdAsync(request.UserId);
            if (!user.IsSuccess)
            {
                return new UnassignUserFromTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { user.Error }
                };
            }
            var project = await projectRepository.FindByIdAsync(taskItem.Value.ProjectId);
            if (!project.IsSuccess)
            {
                return new UnassignUserFromTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { project.Error }
                };
            }
            if(request.OwnerUsername != project.Value.CreatedBy)
            {
                return new UnassignUserFromTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "You are not the owner of this project." }
                };
            }
            var unassignResult = taskItem.Value.UnassignUser();
            if (!unassignResult.IsSuccess)
            {
                return new UnassignUserFromTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { unassignResult.Error }
                };
            }
            var result = await taskItemRepository.UpdateAsync(taskItem.Value);
            if(!result.IsSuccess)
            {
                return new UnassignUserFromTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { result.Error }
                };
            }
            return new UnassignUserFromTaskItemCommandResponse
            {
                Success = true
            };
        }
    }

}
