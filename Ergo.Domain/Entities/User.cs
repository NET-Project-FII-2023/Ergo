using GlobalBuyTicket.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalBuyTicket.Domain.Entities
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

        private User(string firstName, string lastName, string email, string password, UserRole role, List<Project> projects){
            UserId = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Role = role;
            Projects = projects;
        }

        public Guid UserId { get; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public UserRole Role { get; }
        public List<Project>? Projects { get; private set;  }

        public static Result<User> Create(string firstName, string lastName, string email, string password, UserRole role, List<Project> projects)
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

            if (role == default)
            {
                return Result<User>.Failure("Role is required.");
            }


            return Result<User>.Success(new User(firstName, lastName, email, password, role, projects));

        }

        public void AssignProject(Project project)
        {
            if(Projects == null)
            {
                Projects = new List<Project>();
            }   
            Projects.Add(project);
        }

    }
}
