using MediatR;

namespace Ergo.Application.Features.InboxItems.Commands.UpdateInboxItemIsRead
{
    public class UpdateInboxItemIsReadCommand : IRequest<UpdateInboxItemIsReadCommandResponse>
    {
        public Guid InboxItemId { get; set; }
    }
}
