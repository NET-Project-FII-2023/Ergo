using Ergo.Application.Responses;

namespace Ergo.Application.Features.InboxItems.Commands
{
    public class CreateInboxItemCommandResponse : BaseResponse
    {
        public CreateInboxItemCommandResponse() : base()
        {
        }
        public CreateInboxItemDto InboxItem { get; set;}
    }
}