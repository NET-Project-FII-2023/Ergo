using Ergo.Application.Responses;


namespace Ergo.Application.Features.Comments.Commands.UpdateComment
{
    public class UpdateCommentCommandResponse : BaseResponse
    {
        public UpdateCommentCommandResponse() : base()
        {
        }
        public UpdateCommentDto Comment { get; set; }
    }
}
