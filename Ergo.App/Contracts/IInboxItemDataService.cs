using Ergo.App.Services.Responses;
using Ergo.App.ViewModels;

namespace Ergo.App.Contracts;

public interface IInboxItemDataService
{
    Task<List<InboxItemViewModel>> GetInboxItemsByUserIdFromTokenAsync();
}