using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repositories;

public class TaskItemRepository : BaseRepository<TaskItem>, ITaskItemRepository
{
    public TaskItemRepository(ErgoContext context) : base(context)
    {
    }
    public async Task<bool> TaskItemExists(Guid taskItemId)
    {
        return await context.TaskItems.AnyAsync(x => x.TaskItemId == taskItemId);
    }
    public async Task<Result<string>> GetAssignedUser(Guid taskItemId)
    {
        var taskItem = await context.TaskItems
                  .Include(t => t.AssignedUser)
                  .FirstOrDefaultAsync(x => x.TaskItemId == taskItemId);

        if (taskItem == null)
        {
            return Result<string>.Failure($"Task item with id {taskItemId} not found");
        }
        if (taskItem.AssignedUser == null)
        {
            return Result<string>.Failure($"Task item with id {taskItemId} is not assigned to any user");
        }
        return Result<string>.Success(taskItem.AssignedUser.UserId.ToString());
    }
}