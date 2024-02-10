using Ergo.Domain.Common;
using Ergo.Domain.Entities.Enums;

namespace Ergo.Domain.Entities
{
    public class TaskItem : AuditableEntity
    {


        private TaskItem(string taskName, string description, DateTime deadline, string createdBy, Guid projectId, string? branch)
        {
            TaskItemId = Guid.NewGuid();
            TaskName = taskName;
            Description = description;
            Deadline = deadline;
            State = TaskState.ToDo;
            CreatedBy = createdBy;
            CreatedDate = DateTime.UtcNow;
            LastModifiedBy = createdBy;
            LastModifiedDate = DateTime.UtcNow;
            Comments = new List<Comment>();
            ProjectId = projectId;
            AssignedUser = null;
            StartTime = null;
            Branch = branch;
        }
        private TaskItem()
        {

        }
        public User? AssignedUser { get; private set; }
        public Guid TaskItemId { get; private set; }
        public Guid ProjectId { get; set; }
        public string? TaskName { get; private set; }
        public string? Description { get; private set; }
        public DateTime Deadline { get; private set; }
        public List<Comment> Comments { get; set; }
        public TaskState State { get; set; }
        public DateTime? StartTime { get; private set; }
        public TimeSpan TotalTimeSpent { get; private set; } = TimeSpan.Zero;
        public Guid? AssignedUserUserId { get; private set; }


        public string? Branch { get; private set; }

        public static Result<TaskItem> Create(string taskName, string description, DateTime deadline, string createdBy, Guid projectId, string? branch)
        {
            if (string.IsNullOrWhiteSpace(taskName))
            {
                return Result<TaskItem>.Failure(Constants.TaskItemNameRequired);
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                return Result<TaskItem>.Failure(Constants.DescriptionRequired);
            }

            if (deadline == default)
            {
                return Result<TaskItem>.Failure(Constants.DeadlineRequired);
            }
            if (string.IsNullOrWhiteSpace(createdBy))
            {
                return Result<TaskItem>.Failure(Constants.CreatorFullNameRequired);
            }
            if (projectId == Guid.Empty)
            {
                return Result<TaskItem>.Failure(Constants.ProjectIdRequired);
            }



            return Result<TaskItem>.Success(new TaskItem(taskName, description, deadline, createdBy, projectId, branch));
        }

        public Result<TaskItem> StartOrResumeTask()
        {
            if (StartTime.HasValue)
            {
                return Result<TaskItem>.Failure("Task is already in progress");
            }
            StartTime = DateTime.UtcNow;
            return Result<TaskItem>.Success(this);
        }
        public Result<TaskItem> PauseTask()
        {
            if (!StartTime.HasValue)
            {
                return Result<TaskItem>.Failure("Task is not in progress");
            }
            TotalTimeSpent += DateTime.UtcNow - StartTime.Value;
            StartTime = null;
            return Result<TaskItem>.Success(this);
        }
        public Result<TaskItem> AddManualTime(TimeSpan timeToAdd)
        {
            if (timeToAdd == TimeSpan.Zero)
            {
                return Result<TaskItem>.Failure("Time to add cannot be zero");
            }
            TotalTimeSpent += timeToAdd;
            return Result<TaskItem>.Success(this);
        }


        public Result<TaskItem> UpdateData(string taskName, string description, DateTime deadline, string createdBy, Guid projectId, TaskState state, string? branch)
        {
            if (string.IsNullOrWhiteSpace(taskName))
            {
                return Result<TaskItem>.Failure(Constants.TaskItemNameRequired);
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                return Result<TaskItem>.Failure(Constants.DescriptionRequired);
            }
            if (deadline == default)
            {
                return Result<TaskItem>.Failure(Constants.DeadlineRequired);
            }
            if (string.IsNullOrWhiteSpace(createdBy))
            {
                return Result<TaskItem>.Failure(Constants.CreatorFullNameRequired);
            }
            if (projectId == Guid.Empty)
            {
                return Result<TaskItem>.Failure(Constants.ProjectIdRequired);
            }
            if (state == default)
            {
                return Result<TaskItem>.Failure("A valid task state is required");
            }

            TaskName = taskName;
            Description = description;
            Deadline = deadline;
            LastModifiedBy = createdBy;
            LastModifiedDate = DateTime.UtcNow;
            ProjectId = projectId;
            State = state;
            Branch = branch;
            return Result<TaskItem>.Success(this);
        }
        public Result<TaskItem> AssignUser(User user)
        {
            if (user == null)
            {
                return Result<TaskItem>.Failure("User is required");
            }
            AssignedUser = user;
            return Result<TaskItem>.Success(this);
        }
        public Result<TaskItem> DeleteAssignedUser()
        {
            AssignedUserUserId = null;
            return Result<TaskItem>.Success(this);
        }
        public Result<TaskItem> AssignComment(Comment comment)
        {
            if (Comments == null)
            {
                Comments = new List<Comment>();
            }
            if (comment == null)
            {
                return Result<TaskItem>.Failure("Comment is required");
            }
            Comments.Add(comment);
            return Result<TaskItem>.Success(this);
        }


    }
}
