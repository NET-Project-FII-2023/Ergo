using Ergo.Application.Responses;
namespace Ergo.Application.Features.TaskItems.Queries.GetByProjectId
{
    public class GetTasksByProjectIdQueryResponse : BaseResponse
    {
        public List<TaskItemDto> TaskItems { get; set; }
    }
}
