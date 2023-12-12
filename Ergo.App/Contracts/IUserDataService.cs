using Ergo.App.Services.Responses;
using Ergo.App.ViewModels;

namespace Ergo.App.Contracts
{
    public interface IUserDataService
    {
        Task<UserViewModel> GetUserByEmailAsync(string email);
        Task<ApiResponse<UpdateUserDto>> UpdateUserAsync(string id, UpdateUserDto updateUserDto);

    }
}
