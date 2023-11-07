using Ergo.Domain.Entities.Domain.Common;

namespace Ergo.Domain.Entities.Domain.Entities
{
    public class User : AuditableEntity
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
            UserId = System.Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Role = UserRole.ProjectManager;
            UserProjects = new List<UserProject>();
            UserTasks = new List<UserTask>();
        }

        public System.Guid UserId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public UserRole Role { get; private set; }
        public ICollection<UserProject>? UserProjects { get; private set;  }
        public ICollection<UserTask>? UserTasks { get; private set; }

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



            return Result<User>.Success(new User(firstName, lastName, email, password ));

        }

        public void AssignProject(UserTask userTask)
        {
            if(UserTasks == null)
            {
                UserTasks = new List<UserTask>();
            }
            UserTasks.Add(userTask);
        }

        public void AssignTask(UserProject userProject) 
        { 
            if (UserProjects == null) 
            {
                UserProjects = new List<UserProject>(); 
            }
            UserProjects.Add(userProject); 
        }
    }
}
