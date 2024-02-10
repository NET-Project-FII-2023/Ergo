using MediatR;

namespace Ergo.Application.Features.TaskItems.Commands.DeleteAssignedUserFromTask
{
    public class DeleteAssignedUserFromTaskCommand : IRequest<DeleteAssignedUserFromTaskCommandResponse>
    {
        public Guid TaskItemId { get; set; }
        public string Owner { get; set; }
    }
}
