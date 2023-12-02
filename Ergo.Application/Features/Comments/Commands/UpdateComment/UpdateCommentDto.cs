using Ergo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Features.Comments.Commands.UpdateComment
{
    public class UpdateCommentDto
    {
        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; } = default!;

        public DateTime LastModifiedDate { get; set; }
        public string CommentText { get; set; } = default!;
        public Guid TaskId { get; set; } = default!;
    }
}
