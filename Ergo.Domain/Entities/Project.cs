using Ergo.Domain.Common;
using Ergo.Domain.Entities.Enums;

namespace Ergo.Domain.Entities
{
    public class Project : AuditableEntity
    {
        
        private Project(string projectName, string description, DateTime deadline, string fullName)
        {
            ProjectId = Guid.NewGuid();
            ProjectName = projectName;
            Description = description;
            CreatedBy = fullName;
            CreatedDate = DateTime.UtcNow;
            LastModifiedBy = fullName;
            LastModifiedDate = DateTime.UtcNow;
            Deadline = deadline;
            State = ProjectState.JustStarted;
            Members = new List<User>();
        }
        private Project()
        {
            
        }
        public List<User>? Members { get; private set; }
        public Guid ProjectId { get; private set; }
        public string ProjectName { get; private set; }
        public string Description { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime Deadline { get; private set; }
        public ProjectState State { get; private set; }

        public static Result<Project> Create(string projectName, string description, DateTime deadline, string fullName)
        {
            if (string.IsNullOrWhiteSpace(projectName))
            {
                return Result<Project>.Failure(Constants.ProjectNameRequired);
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                return Result<Project>.Failure(Constants.DescriptionRequired);
            }

            if (deadline == default)
            {
                return Result<Project>.Failure(Constants.DeadlineRequired);
            }
            if(string.IsNullOrWhiteSpace(fullName))
            {
                return Result<Project>.Failure(Constants.CreatorFullNameRequired);
            }
            

            return Result<Project>.Success(new Project(projectName, description, deadline, fullName));
        }

        public void AssignMember(User member)
        {
            if (Members == null)
            {
                Members = new List<User>();
            }
        }
    }
}
