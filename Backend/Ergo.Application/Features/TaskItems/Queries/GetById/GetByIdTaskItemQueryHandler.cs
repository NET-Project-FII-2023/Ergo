using Ergo.Application.Features.Users;
using Ergo.Application.Features.Users.Queries;
using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.TaskItems.Queries.GetById
{
    public class GetByIdTaskItemQueryHandler : IRequestHandler<GetByIdTaskItemQuery, GetByIdTaskItemQueryResponse>
    {
        private readonly ITaskItemRepository taskItemRepository;
        private readonly IUserManager userManager;
        private readonly IUserPhotoRepository userPhotoRepository;

        public GetByIdTaskItemQueryHandler(ITaskItemRepository taskItemRepository, IUserManager userManager, IUserPhotoRepository userPhotoRepository)
        {
            this.taskItemRepository = taskItemRepository;
            this.userManager = userManager;
            this.userPhotoRepository = userPhotoRepository;
        }

        public async Task<GetByIdTaskItemQueryResponse> Handle(GetByIdTaskItemQuery request, CancellationToken cancellationToken)
        {
            GetByIdTaskItemQueryResponse response = new();
            var taskItem = await taskItemRepository.FindByIdAsync(request.TaskItemId);

            if (!taskItem.IsSuccess)
            {
                response.Success = false;
                response.ValidationsErrors = new List<string> { taskItem.Error };
                return response;
            }

            UserTaskDto? assignedUser = null;
            var assignedUserId = await taskItemRepository.GetAssignedUser(request.TaskItemId);

            if (assignedUserId.IsSuccess && Guid.TryParse(assignedUserId.Value, out var guidUserId))
            {
                var user = await userManager.FindByIdAsync(guidUserId);

                if (user.IsSuccess)
                {
                    var userPhoto = await userPhotoRepository.GetUserPhotoByUserIdAsync(user.Value.UserId);

                    assignedUser = new UserTaskDto
                    {
                        UserId = user.Value.UserId,
                        Name = user.Value.Name,
                        Username = user.Value.Username,
                        UserPhoto = userPhoto.IsSuccess ? new UserCloudPhotoDto {
                                UserPhotoId = userPhoto.Value.UserPhotoId,
                                PhotoUrl = userPhoto.Value.PhotoUrl
                        } : null
                    };
                }
            }

            return new GetByIdTaskItemQueryResponse
            {
                Success = true,
                TaskItem = new TaskItemDto
                {
                    TaskItemId = taskItem.Value.TaskItemId,
                    ProjectId = taskItem.Value.ProjectId,
                    TaskName = taskItem.Value.TaskName,
                    Description = taskItem.Value.Description,
                    Deadline = taskItem.Value.Deadline,
                    State = taskItem.Value.State,
                    AssignedUser = assignedUser,
                    Branch = taskItem.Value.Branch,
                    StartTime = taskItem.Value.StartTime
                }
            };
        }
    }
}
