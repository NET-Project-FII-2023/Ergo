using Ergo.Application.Responses;


namespace Ergo.Application.Features.Comments.Queries.GetById
{
    public class GetByIdCommentQueryResponse : BaseResponse
    {
        public CommentDto Comment { get; set; }
    }
}
