using Ergo.Domain.Entities;

namespace Ergo.Application.Persistence
{
    public interface ICommentRepository : IAsyncRepository<Comment>
    {
    }
}
