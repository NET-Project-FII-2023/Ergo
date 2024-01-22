using MediatR;

namespace Ergo.Application.Features.TaskItems.Queries.GetTaskItemTime
{
    public class GetTaskItemTimeQuery : IRequest<GetTaskItemTimeQueryResponse>
    {
        public Guid TaskItemId { get; set; }
    }
}
