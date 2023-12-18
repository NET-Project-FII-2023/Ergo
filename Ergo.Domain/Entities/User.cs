using Ergo.Domain.Common;
using Ergo.Domain.Entities.Enums;

namespace Ergo.Domain.Entities
{
    public class User
    {
        private User(Guid userId)
        {
            UserId = userId;
            Projects = new List<Project>();
            Tasks = new List<TaskItem>();
        }

        public Guid UserId { get; private set; }
        public List<Project>? Projects { get; private set; }
        public List<TaskItem> Tasks { get; private set; }

        public static Result<User> Create(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return Result<User>.Failure("UserId cannot be empty");
            }

            return Result<User>.Success(new User(userId));
        }
        public void AssignProject(Project project)
        {
            if (Projects == null)
            {
                Projects = new List<Project>();
            }
            Projects.Add(project);
        }
        public void AssignTask(TaskItem task)
        {
            if (Tasks == null)
            {
                Tasks = new List<TaskItem>();
            }
            Tasks.Add(task);
        }
    }
}
