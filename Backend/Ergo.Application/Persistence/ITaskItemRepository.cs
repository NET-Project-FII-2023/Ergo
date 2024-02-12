using Ergo.Domain.Common;
using Ergo.Domain.Entities;

namespace Ergo.Application.Persistence;

public interface ITaskItemRepository : IAsyncRepository<TaskItem>
{
    Task<bool> TaskItemExists(Guid taskItemId);
    Task<Result<string>> GetAssignedUser(Guid taskItemId);
    Task<int> GetNumberOfTasksByUserIdAsync(Guid userId);
    Task<int> GetTotalHoursWorkedByUserIdAsync(Guid userId);
    Task<Result<IReadOnlyList<TaskItem>>> GetTasksByProjectIdAsync(Guid projectId);
}