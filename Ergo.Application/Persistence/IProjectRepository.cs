using Ergo.Domain.Entities;

namespace Ergo.Application.Persistence;

public interface IProjectRepository : IAsyncRepository<Project>
{
    Task<bool> ProjectExists(Guid projectId);
}