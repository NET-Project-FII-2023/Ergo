using Ergo.Domain.Common;
using Ergo.Domain.Entities.Enums;

namespace Ergo.Domain.Entities
{
    public class TaskItem : AuditableEntity
    {
        

        private TaskItem(string taskName, string description, DateTime deadline, string createdBy, Guid projectId)
        {
            TaskItemId = Guid.NewGuid();
            TaskName = taskName;
            Description = description;
            Deadline = deadline;
            State = TaskState.ToDo;
            CreatedBy = createdBy;
            CreatedDate = DateTime.UtcNow;
            LastModifiedBy = createdBy;
            LastModifiedDate = DateTime.UtcNow;
            Comments = new List<Comment>();
            ProjectId = projectId;
        }
        private TaskItem()
        {
            
        }
        public User AssignedUser { get; private set; }
        public Guid TaskItemId { get; private set; }
        public Guid ProjectId { get; set; }

        public string? TaskName { get; private set; }
        public string? Description { get; private set; }
        public DateTime Deadline { get; private set; }
        public List<Comment> Comments { get; private set; }
        public TaskState State { get; set; }

        public static Result<TaskItem> Create(string taskName, string description, DateTime deadline, string createdBy, Guid projectId)
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
            if(string.IsNullOrWhiteSpace(createdBy))
            {
                return Result<TaskItem>.Failure(Constants.CreatorFullNameRequired);
            }
            if(projectId == Guid.Empty)
            {
                return Result<TaskItem>.Failure(Constants.ProjectIdRequired);
            }



            return Result<TaskItem>.Success(new TaskItem(taskName, description, deadline, createdBy, projectId));
        }

        public void AssignUser(User user)
        {
            AssignedUser = user;
        }

        public void UpdateData(string taskName, string description, DateTime deadline, string createdBy, Guid projectId, TaskState state)
        {
            TaskName = taskName;
            Description = description;
            Deadline = deadline;
            LastModifiedBy = createdBy;
            LastModifiedDate = DateTime.UtcNow;
            ProjectId = projectId;
            State = state;
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
