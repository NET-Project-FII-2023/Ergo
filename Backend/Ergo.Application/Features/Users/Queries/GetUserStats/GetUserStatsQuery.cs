using MediatR;

namespace Ergo.Application.Features.Users.Queries.GetUserStats
{
    public class GetUserStatsQuery : IRequest<GetUserStatsQueryResponse>
    {
        public string UserId { get; set; }
    }
}
