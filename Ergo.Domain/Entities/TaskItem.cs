using Ergo.Domain.Common;

namespace Ergo.Domain.Entities
{
    public class TaskItem : AuditableEntity
    {
        public enum TaskState
        {
            ToDo = 1,
            InProgress = 2,
            Done = 3
        }

        private TaskItem(string taskName, string description, DateTime deadline, Guid createdById,Guid projectId)
        {
            TaskItemId = Guid.NewGuid();
            TaskName = taskName;
            Description = description;
            Deadline = deadline;
            State = TaskState.ToDo;
            CreatedBy = createdById;
            CreatedDate = DateTime.UtcNow;
            LastModifiedBy = createdById;
            LastModifiedDate = DateTime.UtcNow;
            AssignedUser = new List<User>();
            Comments = new List<Comment>();
            
        }
        public TaskItem()
        {
            
        }
        public List<User>? AssignedUser { get; private set; }
        public Guid TaskItemId { get; private set; }
        public Guid ProjectId { get; set; }

        public string? TaskName { get; private set; }
        public string? Description { get; private set; }
        public DateTime Deadline { get; private set; }
        public List<Comment> Comments { get; private set; }
        private TaskState State { get; set; }

        public static Result<TaskItem> Create(string taskName, string description, DateTime deadline, Guid createdById,Guid projectId)
        {
            if (string.IsNullOrWhiteSpace(taskName))
            {
                return Result<TaskItem>.Failure(Constants.TaskItemNameRequired);
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                return Result<TaskItem>.Failure(Constants.DescriptionRequired);
            }

            if (deadline == default)
            {
                return Result<TaskItem>.Failure(Constants.DeadlineRequired);
            }
            if(createdById == Guid.Empty)
            {
                return Result<TaskItem>.Failure(Constants.CreatorIdRequired);
            }
            if(projectId == Guid.Empty)
            {
                return Result<TaskItem>.Failure(Constants.ProjectIdRequired);
            }

            return Result<TaskItem>.Success(new TaskItem(taskName, description, deadline, createdById,projectId));
        }

        public void AssignUser(User user)
        {
            if(AssignedUser == null)
            {
                AssignedUser = new List<User>();
            }
            AssignedUser.Add(user);
        }
        public void AssignComment(Comment comment)
        {
            if (Comments == null)
            {
                Comments = new List<Comment>();
            }
            Comments.Add(comment);
        }


    }
}
