using Ergo.Application.Features.Projects.Queries;
using Ergo.Application.Features.Users.Queries;
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

    public async Task<Result<List<UserProjectDto>>> GetUsersByProjectId(Guid projectId)
    {
        var projectWithUsers = await context.Projects
            .Where(p => p.ProjectId == projectId)
            .SelectMany(p => p.Members)
            .Select(u => new UserProjectDto
            {
                UserId = u.UserId,
            })
            .ToListAsync();
        return Result<List<UserProjectDto>>.Success(projectWithUsers);

    }

    public  Task<bool> DeleteUserFromProjectAsync(Guid projectId, Guid userId)
    {
        var project = context.Projects
            .Include(p => p.Members)
            .FirstOrDefault(p => p.ProjectId == projectId);
        var user = context.Users
            .Include(u => u.Projects)
            .FirstOrDefault(u => u.UserId == userId);
        if (project is null || user is null)
        {
            return Task.FromResult(false);
        }
        project.Members.Remove(user);
        user.Projects.Remove(project);
        context.SaveChanges();
        return Task.FromResult(true);
    }
    public Task<int> GetNumberOfProjectsByUserIdAsync(Guid userId)
    {
        return context.Users
            .Where(u => u.UserId == userId)
            .SelectMany(u => u.Projects)
            .CountAsync();
    }
}