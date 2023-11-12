using Ergo.Domain.Common;
namespace Ergo.Domain.Entities
{
    public class Comment : AuditableEntity
    {
        public Comment(Guid createdById, TaskItem task, string text)
        {
            CommentId = Guid.NewGuid();
            CreatedBy = createdById;
            CreatedDate = DateTime.UtcNow;
            LastModifiedBy = createdById;
            LastModifiedDate = DateTime.UtcNow;
            CommentText = text;
            Task = task;

        }
        public Comment()
        {
            
        }
        public Guid CommentId { get; private set; }
        public string CommentText { get; private set;}
        public TaskItem Task { get; private set; }
        public static Result<Comment> Create(Guid createdById,  TaskItem task, string text)
        {
            if (createdById == Guid.Empty)
            {
                return Result<Comment>.Failure(Constants.CreatorIdRequired);
            }

            if (task == null)
            {
                return Result<Comment>.Failure(Constants.TaskItemRequired);
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return Result<Comment>.Failure(Constants.CommentTextRequired);
            }

            return Result<Comment>.Success(new Comment(createdById, task, text));
        }

    }
}
