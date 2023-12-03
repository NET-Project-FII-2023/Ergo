using Ergo.Application.Persistence;
using Ergo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProjectRepository : BaseRepository<Project>, IProjectRepository
{
    public ProjectRepository(ErgoContext context) : base(context)
    {
    }
    public async Task<bool> ProjectExists(Guid projectId)
    {
        return await context.Projects.AnyAsync(p => p.ProjectId == projectId);
    }
}