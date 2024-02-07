using Ergo.Application.Responses;
namespace Ergo.Application.Features.Users.Queries.Search
{
    public class SearchUsersQueryResponse : BaseResponse
    {
        public UserSearchDto[] Users { get; set; } = [];
    }
}
