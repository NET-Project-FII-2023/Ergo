using Ergo.Domain.Common;
using Ergo.Domain.Entities;

namespace Ergo.Application.Persistence;

public interface ITaskItemRepository : IAsyncRepository<TaskItem>
{
    Task<bool> TaskItemExists(Guid taskItemId);
    Task<Result<string>> GetAssignedUser(Guid taskItemId);
}