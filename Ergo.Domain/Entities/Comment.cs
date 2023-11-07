using Ergo.Domain.Common;

namespace Ergo.Domain.Entities
{
    public class Comment : AuditableEntity
    {
        public Comment(Guid createdById, Task task, string text)
        {
            CommentId = Guid.NewGuid();
            CreatedBy = createdById;
            CreatedDate = DateTime.Now;
            LastModifiedBy = createdById;
            LastModifiedDate = DateTime.Now;
            CommentText = text;
            Task = task;

        }
        public Comment()
        {
            
        }
        public Guid CommentId { get; private set; }
        public string CommentText { get; private set;}
        public Task Task { get; private set; }
        public static Result<Comment> Create(Guid createdById,  Task task, string text)
        {
            if (createdById == Guid.Empty)
            {
                return Result<Comment>.Failure("The user id who created the project is required.");
            }

            if (task == null)
            {
                return Result<Comment>.Failure("Task is required.");
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return Result<Comment>.Failure("Text is required.");
            }

            return Result<Comment>.Success(new Comment(createdById, task, text));
        }

    }
}
