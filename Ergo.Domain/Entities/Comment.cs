using Ergo.Domain.Entities.Domain.Common;
using System;

namespace Ergo.Domain.Entities
{
    public class Comment
    {
        private Comment(Guid userId, Guid taskId, string text)
        {
            CommentId = 
            UserId = userId;
            TaskId = taskId;
            Text = text;
            DateCreated = DateTime.Now;
        }

        // Properties
        public Guid CommentId { get; private set; }
        public Guid UserId { get; private set; } 
        public Guid TaskId { get; private set; } 
        public string Text { get; private set; }
        public DateTime DateCreated { get; private set; }
        public static Result<Comment> Create(Guid userId, Guid taskId, string text)
        {
            if (userId == Guid.Empty)
            {
                return Result<Comment>.Failure("User ID is required.");
            }

            if (taskId == Guid.Empty)
            {
                return Result<Comment>.Failure("Task ID is required.");
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return Result<Comment>.Failure("Text is required.");
            }

            return Result<Comment>.Success(new Comment(userId, taskId, text));
        }
    }
}
