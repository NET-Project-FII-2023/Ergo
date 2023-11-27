using Ergo.Domain.Common;
namespace Ergo.Domain.Entities
{
    public class Comment : AuditableEntity
    {
        public Comment(string fullName, TaskItem task, string text)
        {
            CommentId = Guid.NewGuid();
            CreatedBy = fullName;
            CreatedDate = DateTime.UtcNow;
            LastModifiedBy = fullName;
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
        public static Result<Comment> Create(string fullName,  TaskItem task, string text)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                return Result<Comment>.Failure(Constants.CreatorFullNameRequired);
            }

            if (task == null)
            {
                return Result<Comment>.Failure(Constants.TaskItemRequired);
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return Result<Comment>.Failure(Constants.CommentTextRequired);
            }

            return Result<Comment>.Success(new Comment(fullName, task, text));
        }

        public void UpdateData(string fullName, DateTime createdDate, string lastModifiedBy, DateTime lastModifiedDate, string text, TaskItem task)
        {
            CreatedBy = fullName;
            CreatedDate = createdDate;
            LastModifiedBy = lastModifiedBy;
            LastModifiedDate = lastModifiedDate;
            CommentText = text;
            Task = task;
        }

    }
}
