using Ergo.App.Services.Responses;
using Ergo.App.ViewModels;

namespace Ergo.App.Contracts
{
    public interface ITaskDataService
    {
        Task<List<TaskViewModel>> GetTasksAsync();
        Task<List<TaskViewModel>> GetTasksByProjectIdAsync(Guid projectId);
        Task<ApiResponse<TaskDto>> CreateTaskAsync(TaskViewModel taskViewModel);
        Task<ApiResponse<UpdateTaskDto>> UpdateTaskAsync(UpdateTaskDto updateTaskViewModel);
        Task<ApiResponse<TaskDto>> AssignUserToTaskAsync(Guid taskId, Guid userId);
        Task<string> GetUsernameFromTokenAsync();
    }
}
