using Ergo.Application.Features.InboxItems.Commands.UpdateInboxItemIsRead;
using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;

namespace Infrastructure.Repositories
{
    internal class InboxItemRepository : BaseRepository<InboxItem>, IInboxItemRepository
    {
        public InboxItemRepository(ErgoContext context) : base(context)
        {
        }

        public Task<List<InboxItem>> GetByUserIdAsync(Guid userId)
        {
            return Task.FromResult(context.InboxItems.Where(i => i.UserId == userId).ToList());

        }
        public async Task<Result<InboxItemDto>> UpdateIsReadAsync(InboxItem inboxItem, bool isRead)
        {
            inboxItem.UpdateIsRead(isRead);
            await context.SaveChangesAsync();
            return Result<InboxItemDto>.Success(new InboxItemDto
            {
                UserId = inboxItem.UserId,
                Message = inboxItem.Message,
                CreatedDate = inboxItem.CreatedDate,
                IsRead = inboxItem.IsRead
            });
        }

    }
}
