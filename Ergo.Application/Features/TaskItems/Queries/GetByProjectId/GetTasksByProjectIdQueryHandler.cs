using Ergo.Application.Persistence;
using MediatR;
using Ergo.Application.Features.TaskItems.Queries.GetByProjectId;

namespace Ergo.Application.Features.TaskItems.Queries.GetTasksByProjectId
{
    public class GetTasksByProjectIdQueryHandler : IRequestHandler<GetTasksByProjectIdQuery, GetTasksByProjectIdQueryResponse>
    {
        private readonly ITaskItemRepository _taskItemRepository;

        public GetTasksByProjectIdQueryHandler(ITaskItemRepository taskItemRepository)
        {
            _taskItemRepository = taskItemRepository;
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
                taskItemDtos.Add(new TaskItemDto
                {
                    TaskItemId = taskItem.TaskItemId,
                    BranchId = taskItem.BranchId,
                    TaskName = taskItem.TaskName,
                    Description = taskItem.Description,
                    Deadline = taskItem.Deadline,
                    ProjectId = taskItem.ProjectId,
                    State = taskItem.State
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
