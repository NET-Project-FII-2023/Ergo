using MediatR;

namespace Ergo.Application.Features.TaskItems.Commands.AssignTaskItemToUser
{
    public class AssignTaskItemToUserCommand : IRequest<AssignTaskItemToUserCommandResponse>
    {
        public Guid TaskItemId { get; set; }
        public Guid UserId { get; set; }

    }

}
