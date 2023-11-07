using Ergo.Application.Persistence;
using Ergo.Domain.Entities;

namespace Infrastructure.Repositories;

public class TaskItemRepository : BaseRepository<TaskItem>, ITaskItemRepository
{
    public TaskItemRepository(ErgoContext context) : base(context)
    {
    }
}