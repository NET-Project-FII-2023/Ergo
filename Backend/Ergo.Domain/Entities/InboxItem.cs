using Ergo.Domain.Common;

namespace Ergo.Domain.Entities
{
    public class InboxItem
    {
        private InboxItem(Guid userId, string message)
        {
            InboxItemId = Guid.NewGuid();
            UserId = userId;
            Message = message;
            CreatedDate = DateTime.UtcNow;
            IsRead = false;
        }
        public InboxItem()
        {
            
        }
        public Guid InboxItemId { get; private set; }
        public Guid UserId { get; private set; }
        public string Message { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public bool IsRead { get; private set; }
        public static Result<InboxItem> Create(Guid userId, string message)
        {
            if (userId == default)
            {
                return Result<InboxItem>.Failure(Constants.UserIdRequired);
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                return Result<InboxItem>.Failure(Constants.MessageRequired);
            }
            return Result<InboxItem>.Success(new InboxItem(userId, message));
        }
        public Result<InboxItem> UpdateIsRead(bool isRead)
        {
            IsRead = isRead;
            return Result<InboxItem>.Success(this);
        }

    }
}
