using MediatR;

namespace Ergo.Application.Features.Badges.Queries.GetBadgesForUser
{
    public class GetBadgesForUserQuery : IRequest<GetBadgesForUserQueryResponse>
    {
        public Guid UserId { get; set; }
    }
}
