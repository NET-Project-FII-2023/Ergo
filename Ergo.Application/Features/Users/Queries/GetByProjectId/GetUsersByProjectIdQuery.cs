using MediatR;

namespace Ergo.Application.Features.Users.Queries.GetByProjectId
{
    public class GetUsersByProjectIdQuery : IRequest<GetUsersByProjectIdQueryResponse>
    {
        public string ProjectId { get; set; }
    }
}
