using Ergo.Domain.Entities.Enums;

namespace Ergo.App.ViewModels;

public class UpdateTaskDto : TaskDto
{
    public Guid TaskId { get; set; }
}