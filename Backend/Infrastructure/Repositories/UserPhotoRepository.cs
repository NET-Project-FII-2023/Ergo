using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserPhotoRepository : BaseRepository<UserPhoto>, IUserPhotoRepository
    {
        public UserPhotoRepository(ErgoContext context) : base(context)
        {
        }

        public async Task<Result<UserPhoto>> GetUserPhotoByUserIdAsync(string userId)
        {
            var userPhoto = await context.UserPhotos
                                .Where(up => up.UserId == userId)
                                .FirstOrDefaultAsync();

            if (userPhoto == null)
            {
                return Result<UserPhoto>.Failure("User photo not found");
            }

            return Result<UserPhoto>.Success(userPhoto);

        }
    }
}
