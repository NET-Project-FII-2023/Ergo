using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalBuyTicket.Domain.Common;

namespace GlobalBuyTicket.Domain.Entities
{
    public class Task : AuditableEntity
    {
        public enum TaskState
        {
            ToDo = 1,
            InProgress = 2,
            Done = 3
        }

        private Task(string taskName, string description, DateTime deadline, TaskState state, List<User> assignedUser)
        {
            TaskId = Guid.NewGuid();
            TaskName = taskName;
            Description = description;
            StartDate = DateTime.Now;
            Deadline = deadline;
            State = TaskState.ToDo;
            AssignedUser = assignedUser;
        }

        public List<User> AssignedUser { get; private set; }
        public Guid TaskId { get; private set; }
        public string TaskName { get; private set; }
        public string Description { get; private set; }
        public DateTime StartDate { get; }
        public DateTime Deadline { get; private set; }
        private TaskState State { get; set; }

        public static Result<Task> Create(string taskName, string description, DateTime deadline, List<User> assignedUsers)
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

            return Result<Task>.Success(new Task(taskName, description, deadline, TaskState.ToDo, assignedUsers));
        }

        public void AssignUser(User user)
        {
            if(AssignedUser == null)
            {
                AssignedUser = new List<User>();
            }
            AssignedUser.Add(user);
        }

        public void ChangeState(TaskState state)
        {
            State = state;
        }
    }
}
