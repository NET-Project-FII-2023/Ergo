using Ergo.Application.Features.Projects.Queries;
using Ergo.Application.Persistence;
using Ergo.Domain.Common;
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
    public async Task<Result<List<ProjectDto>>> GetProjectsByUserId(Guid userId)
    {
        var userWithProjects = await context.Users
        .Where(u => u.UserId == userId)
        .SelectMany(u => u.Projects)
        .Select(p => new ProjectDto
        {
            ProjectId = p.ProjectId,
            ProjectName = p.ProjectName,
            Description = p.Description,
            StartDate = p.StartDate,
            Deadline = p.Deadline,
            State = p.State,
            GitRepository = p.GitRepository,
            Members = p.Members
        })
        .ToListAsync();

        return Result<List<ProjectDto>>.Success(userWithProjects);

    }
}