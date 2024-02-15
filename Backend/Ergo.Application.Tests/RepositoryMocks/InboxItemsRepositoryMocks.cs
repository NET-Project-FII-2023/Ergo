using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using NSubstitute;

namespace Ergo.Application.Tests.RepositoryMocks
{
    public static class InboxItemsRepositoryMocks
    {
        internal readonly static List<InboxItem> InboxItems =
            [
                InboxItem.Create(Guid.NewGuid(), "Description1").Value,
                InboxItem.Create(Guid.NewGuid(), "Description2").Value,
            ];
        public static IInboxItemRepository GetInboxItemRepository()
        {
            var mockInboxItemRepository = Substitute.For<IInboxItemRepository>();
            mockInboxItemRepository.GetAllAsync().Returns(Result<IReadOnlyList<InboxItem>>.Success(InboxItems));
            mockInboxItemRepository.FindByIdAsync(InboxItems[0].InboxItemId).Returns(Result<InboxItem>.Success(InboxItems[0]));
            mockInboxItemRepository.FindByIdAsync(InboxItems[1].InboxItemId).Returns(Result<InboxItem>.Success(InboxItems[1]));
            mockInboxItemRepository.FindByIdAsync(Arg.Is<Guid>(id => id != InboxItems[0].InboxItemId && id != InboxItems[1].InboxItemId)).Returns(Result<InboxItem>.Failure("Not found"));
            return mockInboxItemRepository;
        }
    }
}
