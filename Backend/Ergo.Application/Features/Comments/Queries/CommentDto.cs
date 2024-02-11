using Ergo.Application.Features.Users.Queries;
using Ergo.Domain.Entities;

namespace Ergo.Application.Features.Comments.Queries
{
    public class CommentDto
    {
        public Guid CommentId { get; set; }
        public UserCommentDto? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string? CommentText { get; set; }
        public Guid TaskId { get; set; }

    }
}
