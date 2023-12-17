using Ergo.Domain.Common;
namespace Ergo.Domain.Entities
{
    public class Comment : AuditableEntity
    {
        public Comment(string fullName,Guid taskId, string text)
        {
            CommentId = Guid.NewGuid();
            CreatedBy = fullName;
            CreatedDate = DateTime.UtcNow;
            LastModifiedBy = fullName;
            LastModifiedDate = DateTime.UtcNow;
            CommentText = text;
            TaskId = taskId;

        }
        public Comment()
        {
            
        }
        public Guid CommentId { get; private set; }
        public string CommentText { get; private set;}
        public Guid TaskId { get; private set; }
        public static Result<Comment> Create(string fullName,Guid taskId,string text)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                return Result<Comment>.Failure(Constants.CreatorFullNameRequired);
            }

            if(taskId == Guid.Empty)
            {
                return Result<Comment>.Failure(Constants.TaskIdRequired);
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return Result<Comment>.Failure(Constants.CommentTextRequired);
            }

            return Result<Comment>.Success(new Comment(fullName,taskId,text));
        }

        public Result<Comment> UpdateData(string fullName, DateTime createdDate, string lastModifiedBy, DateTime lastModifiedDate, string text,Guid taskId)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                return Result<Comment>.Failure(Constants.CreatorFullNameRequired);
            }
            if (taskId == Guid.Empty)
            {
                   return Result<Comment>.Failure(Constants.TaskIdRequired);
            }
            if (string.IsNullOrWhiteSpace(text))
            {
                return Result<Comment>.Failure(Constants.CommentTextRequired);
            }
            CreatedBy = fullName;
            CreatedDate = createdDate;
            LastModifiedBy = lastModifiedBy;
            LastModifiedDate = lastModifiedDate;
            CommentText = text;
            TaskId = taskId;
            return Result<Comment>.Success(this);
            
        }

    }
}
