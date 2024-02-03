using Ergo.Domain.Common;
using Ergo.Domain.Entities;

namespace Ergo.Application.Persistence
{
    public interface IUserPhotoRepository : IAsyncRepository<UserPhoto>
    {
        Task<Result<string>> GetUserPhotoByUserIdAsync(string userId);

    }
}
