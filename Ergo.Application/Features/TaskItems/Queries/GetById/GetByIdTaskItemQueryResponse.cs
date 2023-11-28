using Ergo.Application.Responses;
using System;

namespace Ergo.Application.Features.TaskItems.Queries.GetById
{
    public class GetByIdTaskItemQueryResponse : BaseResponse
    {
        public TaskItemDto TaskItem { get; set; }
    }
}
