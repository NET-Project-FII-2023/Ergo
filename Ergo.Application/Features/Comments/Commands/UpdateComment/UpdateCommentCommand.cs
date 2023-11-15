using Ergo.Domain.Entities;
using MediatR;

namespace Ergo.Application.Features.Comments.Commands.UpdateComment
{
    public class UpdateCommentCommand :IRequest<UpdateCommentCommandResponse>
    {
        public Guid CommentId { get; set; }
        
        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; } = default!;
        public DateTime LastModifiedDate { get; set; }
        public string CommentText { get; set; } = default!;
        public TaskItem Task { get; set; } = default!;
        

    }
}
