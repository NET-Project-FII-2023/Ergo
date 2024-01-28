using MediatR;

namespace Ergo.Application.Features.TaskItems.Commands.PauseTimerTaskItem
{
    public class PauseTimerTaskItemCommand : IRequest<PauseTimerTaskItemCommandResponse>
    {
        public Guid TaskItemId { get; set; }
        public Guid UserId { get; set; }
    }
}
