namespace Ergo.Application.Features.InboxItems.Commands
{
    public class CreateInboxItemDto
    {
        public Guid UserId { get;  set; }
        public string Message { get;  set; }
        public DateTime CreatedDate { get;  set; }
        public bool IsRead { get;  set; }
    }
}