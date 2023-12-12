using Ergo.Application.Responses;
using System;
using System.Collections.Generic;

namespace Ergo.Application.Features.TaskItems.Queries.GetTasksByProjectId
{
    public class GetTasksByProjectIdQueryResponse : BaseResponse
    {
        public List<TaskItemDto> TaskItems { get; set; }
    }
}
