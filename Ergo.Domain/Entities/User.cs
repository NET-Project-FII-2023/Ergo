using Ergo.Domain.Common;

namespace Ergo.Domain.Entities
{
    public class User 
    {
        public enum UserRole
        {
            ProjectManager = 1,
            Developer = 2,
            Tester = 3,
            DevOps = 4,
            BusinessAnalyst = 5,
            ScrumMaster = 6,
            ProductOwner = 7,
            TeamLead = 8
        }

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
                return Result<User>.Failure("First Name is required.");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                return Result<User>.Failure("Last Name is required.");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                return Result<User>.Failure("Email is required.");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return Result<User>.Failure("Password is required.");
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
