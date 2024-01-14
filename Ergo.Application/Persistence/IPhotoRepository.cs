using Ergo.Domain.Entities;

namespace Ergo.Application.Persistence
{
    public interface IPhotoRepository : IAsyncRepository<Photo>
    {
        Task<List<Photo>> GetByTaskItemIdAsync(Guid taskItemId);
    }
}
