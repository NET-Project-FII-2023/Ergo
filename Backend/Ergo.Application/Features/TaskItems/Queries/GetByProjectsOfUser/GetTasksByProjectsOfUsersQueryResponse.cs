using Ergo.Application.Responses;

namespace Ergo.Application.Features.TaskItems.Queries.GetByUserId;

public class GetTasksByProjectsOfUsersQueryResponse : BaseResponse
{   
    public Dictionary<Guid, List<TaskItemDto>> TaskItems { get; set; }
}