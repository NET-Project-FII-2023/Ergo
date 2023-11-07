using Ergo.Domain.Entities.Domain.Common;
using Ergo.Domain.Entities.Domain.Entities;

namespace Ergo.Domain.Entities
{
    public class Project : AuditableEntity
    {
      

        public enum ProjectState
        {
            JustStarted = 1,
            Development = 2,
            Testing = 3,
            Production,
            Done = 5
        }
        private Project(string projectName, string description, DateTime deadline)
        {
            ProjectId = System.Guid.NewGuid();
            ProjectName = projectName;
            Description = description;
            StartDate = DateTime.Now;
            Deadline = deadline;
            State = ProjectState.JustStarted;
            UsersAssigned = new List<Users>();
        }

       
        public System.Guid ProjectId { get; set; }
        public string ProjectName { get; private set; }
        public string Description { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime Deadline { get; private set; }
        public ProjectState State { get; private set; }
        public ICollection<User> UsersAssigned { get; private set; }

        public static Result<Project> Create(string projectName, string description, DateTime deadline,
            List<User> members)
        {
            if (string.IsNullOrWhiteSpace(projectName))
            {
                return Result<Project>.Failure("Project Name is required.");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                return Result<Project>.Failure("Description is required.");
            }

            if (deadline == default)
            {
                return Result<Project>.Failure("Deadline is required.");
            }

            return Result<Project>.Success(new Project(projectName, description, deadline));
        }

        public void AssignMember(User user)
        {
            if (UsersAssigned == null)
            {
                UsersAssigned = new List<User>();
            }
            if (!UsersAssigned.Contains(user))
            {
                UsersAssigned.Add(user);
            }
        }
    }
}
