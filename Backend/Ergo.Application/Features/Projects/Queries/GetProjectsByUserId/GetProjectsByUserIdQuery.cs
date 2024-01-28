using MediatR;

namespace Ergo.Application.Features.Projects.Queries.GetProjectsByUserId
{
    public class GetProjectsByUserIdQuery : IRequest<GetProjectsByUserIdQueryResponse>
    {
        public string UserId { get; set; }
    }
}
