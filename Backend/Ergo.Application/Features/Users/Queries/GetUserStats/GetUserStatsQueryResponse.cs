using Ergo.Application.Responses;
using MediatR;

namespace Ergo.Application.Features.Users.Queries.GetUserStats
{
    public class GetUserStatsQueryResponse : BaseResponse
    {
        public GetUserStatsQueryResponse() : base()
        {
        }
        public UserStatsDto? UserStats { get; set; }

    }
}