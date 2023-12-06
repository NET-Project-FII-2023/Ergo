using MediatR;

namespace Ergo.Application.Features.Comments.Commands.CreateComment
{
    public class CreateCommentCommand : IRequest<CreateCommentCommandResponse>
    {
        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; } = default!;

        public DateTime LastModifiedDate { get; set; }
        public string CommentText { get; set; } = default!;
        public Guid TaskId { get; set; }
    }
}
