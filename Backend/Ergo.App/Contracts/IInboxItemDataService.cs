using Ergo.App.Services.Responses;
using Ergo.App.ViewModels;

namespace Ergo.App.Contracts;

public interface IInboxItemDataService
{
    Task<List<InboxItemViewModel>> GetInboxItemsByUserIdFromTokenAsync();
    Task<InboxItemViewModel> UpdateInboxItemAsync(InboxItemViewModel inboxItemViewModel);
    Task<ApiResponse<InboxItemViewModel>> CreateInboxItemAsync(InboxItemViewModel inboxItemViewModel);
}