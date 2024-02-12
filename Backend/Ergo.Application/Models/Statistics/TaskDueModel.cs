using Ergo.Domain.Entities.Enums;

namespace Ergo.Application.Models.Statistics;

public class TaskDueModel
{
    public Guid TaskItemId { get; set; }
    public Guid ProjectId { get; set; }
    public DateTime Deadline { get; set; }
    public TaskState State { get; set; }
}