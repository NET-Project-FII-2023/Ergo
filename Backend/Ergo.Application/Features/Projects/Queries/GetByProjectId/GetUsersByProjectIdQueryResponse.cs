using Ergo.Application.Responses;

namespace Ergo.Application.Features.Users.Queries.GetByProjectId
{
    public class GetUsersByProjectIdQueryResponse : BaseResponse
    {
        public GetUsersByProjectIdQueryResponse() : base()
        {
        }

        public List<UserDto> Users { get; set; }
    }
}