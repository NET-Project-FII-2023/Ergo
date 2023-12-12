using Ergo.App.ViewModels;

namespace Ergo.App.Contracts
{
    public interface IUserDataService
    {
        Task<UserViewModel> GetUserByEmailAsync(string email);

    }
}
