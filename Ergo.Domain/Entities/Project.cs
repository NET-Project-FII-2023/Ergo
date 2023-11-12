﻿using Ergo.Domain.Common;

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
        private Project(string projectName, string description, DateTime deadline, Guid createdById)
        {
            ProjectId = Guid.NewGuid();
            ProjectName = projectName;
            Description = description;
            CreatedBy = createdById;
            CreatedDate = DateTime.UtcNow;
            LastModifiedBy = createdById;
            LastModifiedDate = DateTime.UtcNow;
            Deadline = deadline;
            State = ProjectState.JustStarted;
            Members = new List<User>();
        }
        public Project()
        {
            
        }
        public List<User>? Members { get; private set; }
        public Guid ProjectId { get; private set; }
        public string ProjectName { get; private set; }
        public string Description { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime Deadline { get; private set; }
        public ProjectState State { get; private set; }

        public static Result<Project> Create(string projectName, string description, DateTime deadline, Guid createdById)
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
            if(createdById == Guid.Empty)
            {
                return Result<Project>.Failure(Constants.CreatorIdRequired);
            }
            

            return Result<Project>.Success(new Project(projectName, description, deadline,createdById));
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
