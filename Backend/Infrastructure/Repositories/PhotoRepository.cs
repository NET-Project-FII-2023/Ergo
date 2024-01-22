using Ergo.Application.Persistence;
using Ergo.Domain.Entities;

namespace Infrastructure.Repositories
{
    public class PhotoRepository : BaseRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(ErgoContext context) : base(context)
        {

        }

        public  Task<List<Photo>> GetByTaskItemIdAsync(Guid taskItemId)
        {
            return Task.FromResult(context.Photos.Where(p => p.TaskItemId == taskItemId).ToList());
        }
    }
}
