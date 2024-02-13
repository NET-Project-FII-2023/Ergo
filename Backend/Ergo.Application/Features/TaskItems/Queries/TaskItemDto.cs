using Ergo.Application.Features.Users.Queries;
using Ergo.Domain.Entities.Enums;

namespace Ergo.Application.Features.TaskItems.Queries
{
    public class TaskItemDto
    {
        public Guid TaskItemId { get; set; }
        public string? BranchId { get; set; }
        public string? TaskName { get; set; }
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
        public string? CreatedBy { get; set; }
        public Guid ProjectId { get; set; }
        public UserTaskDto? AssignedUser { get; set; }
        public TaskFileDto[] TaskFiles { get; set; }
        public TaskState State { get; set; }
        public string? Branch { get; set; }
        public DateTime? StartTime { get; set; }
    }
}
