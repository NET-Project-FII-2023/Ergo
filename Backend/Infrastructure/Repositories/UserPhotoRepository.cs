using Ergo.Application.Persistence;
using Ergo.Domain.Entities;

namespace Infrastructure.Repositories
{
    public class UserPhotoRepository : BaseRepository<UserPhoto>, IUserPhotoRepository
    {
        public UserPhotoRepository(ErgoContext context):base(context)
        { 
        }

    }
}
