using Ergo.Application.Persistence;
using Ergo.Domain.Entities;

namespace Infrastructure.Repositories;

public class ProjectRepository : BaseRepository<Project>, IProjectRepository
{
    public ProjectRepository(ErgoContext context) : base(context)
    {
    }
}