using Ergo.Domain.Entities;

namespace Ergo.Application.Persistence;

public interface ITaskItemRepository : IAsyncRepository<TaskItem>
{
    Task<bool> TaskItemExists(Guid taskItemId);
}