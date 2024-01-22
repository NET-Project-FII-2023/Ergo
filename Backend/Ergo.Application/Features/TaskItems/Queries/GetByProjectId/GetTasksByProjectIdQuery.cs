using MediatR;

namespace Ergo.Application.Features.TaskItems.Queries.GetByProjectId
{
    public class GetTasksByProjectIdQuery : IRequest<GetTasksByProjectIdQueryResponse>
    {
        public Guid ProjectId { get; set; }
    }
}
