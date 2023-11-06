using GlobalBuyTicket.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalBuyTicket.Domain.Entities
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
        private Project(string projectName, string description, DateTime deadline, List<User> members)
        {
            ProjectId = Guid.NewGuid();
            ProjectName = projectName;
            Description = description;
            StartDate = DateTime.Now;
            Deadline = deadline;
            State = ProjectState.JustStarted;
            Members = members;
        }

        public List<User>? Members { get; private set; }
        public Guid ProjectId { get; }
        public string ProjectName { get; private set; }
        public string Description { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime Deadline { get; private set; }
        public ProjectState State { get; private set; }

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

            return Result<Project>.Success(new Project(projectName, description, deadline, members));
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
