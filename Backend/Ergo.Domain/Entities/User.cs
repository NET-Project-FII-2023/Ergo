using Ergo.Domain.Common;

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
        public Result<User> AssignProject(Project project)
        {
            if(project == null)
            {
                return Result<User>.Failure("Project cannot be null");
            }
            Projects.Add(project);
            return Result<User>.Success(this);
           
        }
        public Result<User> AssignTask(TaskItem task)
        {
            if (task == null)
            {
                return Result<User>.Failure("Task cannot be null");
            }
            Tasks.Add(task);
            return Result<User>.Success(this);
        }
    }
}
