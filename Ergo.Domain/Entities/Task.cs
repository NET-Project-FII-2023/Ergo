using Ergo.Domain.Entities.Domain.Common;

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

        private Task(string taskName, string description, DateTime deadline)
        {
            TaskId = System.Guid.NewGuid();
            TaskName = taskName;
            Description = description;
            StartDate = DateTime.Now;
            Deadline = deadline;
            State = TaskState.ToDo;
            AssignedTo = new List<Guid>();
        }

        public System.Guid TaskId { get;  private set; }
        public string TaskName { get; private set; }
        public string Description { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime Deadline { get; private set; }
        public TaskState State { get; private set; }
        public ICollection<User> AssignedTo { get; private set; }


        public static Result<Task> Create(string taskName, string description, DateTime deadline)
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

            return Result<Task>.Success(new Task(taskName, description, deadline));
        }

        public void AssignUser(Guid user)
        {
            if(AssignedTo == null)
            {
                AssignedTo = new List<Guid>();
            }
            if(!AssignedTo.Contains(user))
            {
                AssignedTo.Add(user);
            }
        }

        public void ChangeState(TaskState state)
        {
            State = state;
        }
    }
}
