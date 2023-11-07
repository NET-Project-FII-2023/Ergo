using Ergo.Domain.Common;
using System.Xml.Linq;

namespace Ergo.Domain.Entities
{
    public class Task : AuditableEntity
    {
        public enum TaskState
        {
            ToDo = 1,
            InProgress = 2,
            Done = 3
        }

        private Task(string taskName, string description, DateTime deadline, Guid createdById,Guid projectId)
        {
            TaskId = Guid.NewGuid();
            TaskName = taskName;
            Description = description;
            Deadline = deadline;
            State = TaskState.ToDo;
            CreatedBy = createdById;
            CreatedDate = DateTime.Now;
            LastModifiedBy = createdById;
            LastModifiedDate = DateTime.Now;
            AssignedUser = new List<User>();
            Comments = new List<Comment>();
            
        }
        public Task()
        {
            
        }
        public List<User>? AssignedUser { get; private set; }
        public Guid TaskId { get; private set; }
        public Guid ProjectId { get; set; }

        public string? TaskName { get; private set; }
        public string? Description { get; private set; }
        public DateTime Deadline { get; private set; }
        public List<Comment> Comments { get; private set; }
        private TaskState State { get; set; }

        public static Result<Task> Create(string taskName, string description, DateTime deadline, Guid createdById,Guid projectId)
        {
            if (string.IsNullOrWhiteSpace(taskName))
            {
                return Result<Task>.Failure("Task Name is required.");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                return Result<Task>.Failure("Description is required.");
            }

            if (deadline == default)
            {
                return Result<Task>.Failure("Deadline is required.");
            }
            if(createdById == Guid.Empty)
            {
                return Result<Task>.Failure("The user id who created the project is required.");
            }
            if(projectId == Guid.Empty)
            {
                return Result<Task>.Failure("The project id is required.");
            }

            return Result<Task>.Success(new Task(taskName, description, deadline, createdById,projectId));
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
