using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.TaskItems.Queries.GetAll
{
    public class GetAllTaskItemsQueryHandler : IRequestHandler<GetAllTaskItemsQuery, GetAllTaskItemsResponse>
    {
        private readonly ITaskItemRepository taskItemRepository;
        public GetAllTaskItemsQueryHandler(ITaskItemRepository taskItemRepository)
        {
            this.taskItemRepository = taskItemRepository;
        }

        public async Task<GetAllTaskItemsResponse> Handle(GetAllTaskItemsQuery request,
            CancellationToken cancellationToken)
        {
            GetAllTaskItemsResponse response = new();
            var result = await taskItemRepository.GetAllAsync();
            if (result.IsSuccess)
            {
                response.TaskItems = result.Value.Select(t => new TaskItemDto
                {
                    TaskItemId = t.TaskItemId,
                    TaskName = t.TaskName,
                    Description = t.Description,
                    Deadline = t.Deadline,
                    CreatedBy = t.CreatedBy,
                    ProjectId = t.ProjectId,
                    State = t.State
                   
                }).ToList();
            }
            return response;
        }

    }
}
