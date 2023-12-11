using Ergo.App.Services.Responses;
using Ergo.App.ViewModels;

namespace Ergo.App.Contracts
{
    public interface ITaskDataService
    {
        Task<List<TaskViewModel>> GetTasksAsync();

        Task<ApiResponse<TaskDto>> CreateTaskAsync(TaskViewModel categoryViewModel);
    }
}
