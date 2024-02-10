using Ergo.Application.Features.Users.Queries;
using Ergo.Application.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ergo.Application.Features.TaskItems.Queries.GetAll
{
    public class GetAllTaskItemsQueryHandler : IRequestHandler<GetAllTaskItemsQuery, GetAllTaskItemsResponse>
    {
        private readonly ITaskItemRepository taskItemRepository;
        private readonly IUserManager userManager;
        public GetAllTaskItemsQueryHandler(ITaskItemRepository taskItemRepository, IUserManager userManager)
        {
            this.taskItemRepository = taskItemRepository;
            this.userManager = userManager;
        }

        public async Task<GetAllTaskItemsResponse> Handle(GetAllTaskItemsQuery request,
            CancellationToken cancellationToken)
        {
            GetAllTaskItemsResponse response = new();
            var result = await taskItemRepository.GetAllAsync();
            if (result.IsSuccess)
            {
                response.TaskItems = new List<TaskItemDto>();
                foreach (var taskItem in result.Value)
                {
                    UserTaskDto? assignedUser = null;
                    var assignedUserId = await taskItemRepository.GetAssignedUser(taskItem.TaskItemId);

                    if (assignedUserId.IsSuccess)
                    {
                        if (Guid.TryParse(assignedUserId.Value, out var guidUserId))
                        {
                            var user = await userManager.FindByIdAsync(guidUserId);

                            if (user.IsSuccess)
                            {
                                assignedUser = new UserTaskDto
                                {
                                    UserId = user.Value.UserId,
                                    Name = user.Value.Name,
                                    Username = user.Value.Username,
                                };
                            }
                        }
                    }
                    response.TaskItems.Add(new TaskItemDto
                    {
                        TaskItemId = taskItem.TaskItemId,
                        ProjectId = taskItem.ProjectId,
                        TaskName = taskItem.TaskName,
                        Description = taskItem.Description,
                        Deadline = taskItem.Deadline,
                        CreatedBy = taskItem.CreatedBy,
                        AssignedUser = assignedUser,
                        State = taskItem.State,
                        Branch = taskItem.Branch
                    });
                }
            }
            return response;
        }

    }
}
