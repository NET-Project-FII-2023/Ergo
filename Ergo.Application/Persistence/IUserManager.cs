using Ergo.Application.Features.Users.Queries;
using Ergo.Domain.Common;

namespace Ergo.Application.Persistence
{
    public interface IUserManager
    {
        Task<Result<UserDto>> FindByIdAsync(Guid userId);
        Task<Result<List<UserDto>>> GetAllAsync();
        Task<Result<UserDto>> DeleteAsync(Guid userId);
    }
}
