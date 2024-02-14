using Ergo.Domain.Common;
using Ergo.Domain.Entities;

namespace Ergo.Application.Persistence
{
    public interface ICommentRepository : IAsyncRepository<Comment>
    {
        Task<int> GetNumberOfCommentsByUserIdAsync(string name);
        Task<Result<IReadOnlyList<Comment>>> GetCommentByTaskIdAsync(Guid taskId);
    }
}
