using Ergo.Application.Features.InboxItems.Commands.UpdateInboxItemIsRead;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;

namespace Ergo.Application.Persistence
{
    public interface IInboxItemRepository : IAsyncRepository<InboxItem>
    {
        Task<List<InboxItem>> GetByUserIdAsync(Guid userId);
        Task<Result<InboxItemDto>> UpdateIsReadAsync(InboxItem inboxItem, bool isRead);
    }
}
