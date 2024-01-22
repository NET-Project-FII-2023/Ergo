using MediatR;

namespace Ergo.Application.Features.Comments.Commands.DeleteComment
{
    public class DeleteCommentCommand : IRequest<DeleteCommentCommandResponse>
    {
        public Guid CommentId { get; set; }
    }
}
