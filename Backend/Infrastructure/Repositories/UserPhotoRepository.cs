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

        public async Task<Result<string>> GetUserPhotoByUserId(string userId)
        {
            var userPhoto = await context.UserPhotos
                                .Where(up => up.UserId == userId)
                                .Select(up => up.PhotoUrl)
                                .FirstOrDefaultAsync();
            if (userPhoto == null)
            {
                return Result<string>.Failure("User photo not found");
            }
            return Result<string>.Success(userPhoto);

        }
    }
}
