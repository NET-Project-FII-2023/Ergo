using Ergo.Domain.Common;
using Ergo.Domain.Entities.Enums;

namespace Ergo.Domain.Entities
{
    public class User 
    {
        private User(string firstName, string lastName, string email, string password){
            UserId = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Role = UserRole.Developer;
            Projects = new List<Project>();
            Tasks = new List<TaskItem>();
        }

        public Guid UserId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public UserRole Role { get; }
        public List<Project>? Projects { get; private set;  }
        public List<TaskItem> Tasks { get; private set; }

        public static Result<User> Create(string firstName, string lastName, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                return Result<User>.Failure(Constants.FirstNameRequired);
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                return Result<User>.Failure(Constants.LastNameRequired);
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                return Result<User>.Failure(Constants.EmailRequired);
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return Result<User>.Failure(Constants.PasswordRequired);
            }


            return Result<User>.Success(new User(firstName, lastName, email, password));

        }

        public void AssignProject(Project project)
        {
            if(Projects == null)
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
