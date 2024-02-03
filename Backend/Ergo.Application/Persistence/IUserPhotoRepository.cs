using Ergo.Domain.Common;
using Ergo.Domain.Entities;

namespace Ergo.Application.Persistence
{
    public interface IUserPhotoRepository : IAsyncRepository<UserPhoto>
    {
        //method to get a user photo by user id
        Task<Result<string>> GetUserPhotoByUserId(string userId);

    }
}
