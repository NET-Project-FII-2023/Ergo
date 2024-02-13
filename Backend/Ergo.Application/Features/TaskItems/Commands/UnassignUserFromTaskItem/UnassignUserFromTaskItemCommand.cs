using MediatR;

namespace Ergo.Application.Features.TaskItems.Commands.UnassignUserFromTaskItem
{
    public class UnassignUserFromTaskItemCommand : IRequest<UnassignUserFromTaskItemCommandResponse>
    {
        public Guid TaskItemId { get; set; }
        public Guid UserId { get; set; }
        public string OwnerUsername { get; set; }
    }
}
