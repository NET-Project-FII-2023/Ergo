using Ergo.Application.Persistence;
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
}