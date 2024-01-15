using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.TaskItems.Queries.GetTaskItemTime
{
    public class GetTaskItemTimeQueryHandler : IRequestHandler<GetTaskItemTimeQuery, GetTaskItemTimeQueryResponse>
    {
        private readonly ITaskItemRepository taskItemRepository;
        public GetTaskItemTimeQueryHandler(ITaskItemRepository taskItemRepository)
        {
            this.taskItemRepository = taskItemRepository;
        }

        public async Task<GetTaskItemTimeQueryResponse> Handle(GetTaskItemTimeQuery request, CancellationToken cancellationToken)
        {
            var taskItem = await taskItemRepository.FindByIdAsync(request.TaskItemId);
            if(!taskItem.IsSuccess)
            {
                return new GetTaskItemTimeQueryResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "TaskItem with the provided ID does not exist." }
                };
            }
            return new GetTaskItemTimeQueryResponse
            {
                Success = true,
                RecordedTime = taskItem.Value.TotalTimeSpent
            };
        }
    }
}
