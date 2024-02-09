using Ergo.Domain.Entities.Enums;
using MediatR;

namespace Ergo.Application.Features.TaskItems.Commands.CreateTaskItem
{
    public class CreateTaskItemCommand : IRequest<CreateTaskItemCommandResponse>
    {
        public string TaskName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime Deadline { get; set; } = default!;
        public string CreatedBy { get; set; } = default!;
        public Guid ProjectId { get; set; }
        public TaskState? State { get; set; } = TaskState.ToDo;
        public string? Branch { get; set; } = default!;
    }
}
