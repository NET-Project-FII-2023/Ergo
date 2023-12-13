using Ergo.App.Services.Responses;
using Ergo.App.ViewModels;

namespace Ergo.App.Contracts
{
    public interface IProjectDataService
    {
        Task<List<ProjectViewModel>> GetProjectsAsync();

        Task<ApiResponse<ProjectDto>> CreateProjectAsync(ProjectViewModel projectViewModel);
    }
}
