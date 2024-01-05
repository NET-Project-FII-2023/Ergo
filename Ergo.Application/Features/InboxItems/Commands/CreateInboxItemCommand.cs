using MediatR;

namespace Ergo.Application.Features.InboxItems.Commands
{
    public class CreateInboxItemCommand : IRequest<CreateInboxItemCommandResponse>
    {
        public Guid UserId { get; set; }
        public string Message { get; set; } = default!;
    }
}
