using MediatR;

namespace Ergo.Application.Features.TaskItems.Commands.StartTimerTaskItem
{
    public class StartTimerTaskItemCommand : IRequest<StartTimerTaskItemCommandResponse>
    {
        public Guid TaskItemId { get; set; }
        public Guid UserId { get; set; }
    }
}
