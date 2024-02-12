using Ergo.Application.Responses;

namespace Ergo.Application.Features.Comments.Queries.GetAll
{
    public class GetAllCommentsResponse : BaseResponse
    {
        public List<CommentDto> Comments { get; set; }
    }
}
