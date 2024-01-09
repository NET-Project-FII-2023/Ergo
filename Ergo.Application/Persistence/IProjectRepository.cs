using Ergo.Application.Features.Projects.Queries;
using Ergo.Application.Features.Users.Queries;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;

namespace Ergo.Application.Persistence;

public interface IProjectRepository : IAsyncRepository<Project>
{
    Task<bool> ProjectExists(Guid projectId);
    Task<Result<List<ProjectDto>>> GetProjectsByUserId(Guid projectId);
    Task<Result<List<UserProjectDto>>> GetUsersByProjectId(Guid projectId);
    Task<bool> DeleteUserFromProjectAsync(Guid projectId, Guid userId);


}