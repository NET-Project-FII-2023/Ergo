using Ergo.Domain.Common;

namespace Ergo.Domain.Entities
{
    public class UserPhoto
    {
        private UserPhoto(string photoUrl, string userId)
        {
            UserPhotoId = Guid.NewGuid();
            PhotoUrl = photoUrl;
            UserId = userId;
        }
        private UserPhoto()
        {
            
        }
        public Guid UserPhotoId { get; private set; }
        public string PhotoUrl { get; private set; }
        public string UserId { get; private set; }
        public static Result<UserPhoto> Create(string photoUrl, string userId)
        {
            if (string.IsNullOrWhiteSpace(photoUrl))
            {
                return Result<UserPhoto>.Failure(Constants.PhotoUrlRequired);
            }
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Result<UserPhoto>.Failure(Constants.UserIdRequired);
            }
            return Result<UserPhoto>.Success(new UserPhoto(photoUrl, userId));
        }

        public Result<UserPhoto> UpdatePhoto(string photoUrl)
        {
            if (string.IsNullOrWhiteSpace(photoUrl))
            {
                return Result<UserPhoto>.Failure(Constants.PhotoUrlRequired);
            }
            PhotoUrl = photoUrl;
            return Result<UserPhoto>.Success(this);
        }
    }
}
