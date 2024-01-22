using MediatR;
using System;

namespace Ergo.Application.Features.TaskItems.Commands.DeleteTaskItem
{
    public class DeleteTaskItemCommand : IRequest<DeleteTaskItemCommandResponse>
    {
        public Guid TaskItemId { get; set; }
    }
}
