using Ergo.Application.Features.Users.Queries;
using Ergo.Application.Persistence;
using MediatR;
namespace Ergo.Application.Features.TaskItems.Queries.GetByProjectId
{
    public class GetTasksByProjectIdQueryHandler : IRequestHandler<GetTasksByProjectIdQuery, GetTasksByProjectIdQueryResponse>
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly IUserManager _userManager;
        private readonly IPhotoRepository _photoRepository;

        public GetTasksByProjectIdQueryHandler(ITaskItemRepository taskItemRepository, IUserManager userManager, IPhotoRepository photoRepository)
        {
            _taskItemRepository = taskItemRepository;
            _userManager = userManager;
            _photoRepository = photoRepository;
        }

        public async Task<GetTasksByProjectIdQueryResponse> Handle(GetTasksByProjectIdQuery request, CancellationToken cancellationToken)
        {
            //var projectExists = await _taskItemRepository.FindByIdAsync(request.ProjectId);
            //if(!projectExists.IsSuccess)
            //{
            //    return new GetTasksByProjectIdQueryResponse
            //    {
            //        Success = false,
            //        ValidationsErrors = new List<string> { "Project with the provided ID does not exist." }
            //    };
            //}
            var taskItemsResult = await _taskItemRepository.GetTasksByProjectIdAsync(request.ProjectId);

            if (!taskItemsResult.IsSuccess)
            {
                // Handle the case where fetching task items by project ID fails
                return new GetTasksByProjectIdQueryResponse
                {
                    Success = false,
                    // Handle error or return empty task items list
                    // TaskItems = new List<TaskItemDto>() or TaskItems = null
                };
            }

            var taskItems = taskItemsResult.Value;

            var taskItemDtos = new List<TaskItemDto>();

            foreach (var taskItem in taskItems)
            {
                UserTaskDto? assignedUser = null;
                var assignedUserId = await _taskItemRepository.GetAssignedUser(taskItem.TaskItemId);

                if (assignedUserId.IsSuccess)
                {
                    if (Guid.TryParse(assignedUserId.Value, out var guidUserId))
                    {
                        var user = await _userManager.FindByIdAsync(guidUserId);

                        if (user.IsSuccess)
                        {
                            assignedUser = new UserTaskDto
                            {
                                UserId = user.Value.UserId,
                                Name = user.Value.Name,
                                Username = user.Value.Username,
                                UserPhoto = user.Value.UserPhoto
                            };
                        }
                    }
                }

                var taskFiles = await _photoRepository.GetByTaskItemIdAsync(taskItem.TaskItemId);
                taskItemDtos.Add(new TaskItemDto
                {
                    TaskItemId = taskItem.TaskItemId,
                    TaskName = taskItem.TaskName,
                    Description = taskItem.Description,
                    Deadline = taskItem.Deadline,
                    ProjectId = taskItem.ProjectId,
                    State = taskItem.State,
                    AssignedUser = assignedUser,
                    TaskFiles = taskFiles.Select(photo => new TaskFileDto
                    {
                        TaskFileId = photo.PhotoId,
                        FileUrl = photo.CloudURL
                    }).ToArray(),
                    Branch = taskItem.Branch
                });
            }

            return new GetTasksByProjectIdQueryResponse
            {
                Success = true,
                TaskItems = taskItemDtos
            };
        }

    }
}
