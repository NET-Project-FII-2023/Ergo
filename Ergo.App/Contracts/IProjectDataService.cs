using Ergo.App.Services.Responses;
using Ergo.App.ViewModels;

namespace Ergo.App.Contracts
{
    public interface IProjectDataService
    {
        Task<List<ProjectViewModel>> GetProjectsAsync();
        Task<ApiResponse<ProjectDto>> GetProjectByIdAsync(Guid id);

        Task<ApiResponse<ProjectDto>> CreateProjectAsync(ProjectViewModel projectViewModel);
        Task<List<ProjectViewModel>> GetProjectsByUserIdFromTokenAsync();
        Task<List<ProjectViewModel>> GetProjectsByUserIdAsync(string userId);

        Task<ApiResponse<ProjectDto>> AssignUserToProjectAsync(ProjectAssignUserViewModel projectAssignUserViewModel);
        
    }
}
