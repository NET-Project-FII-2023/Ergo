using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BaseRepository<T> : IAsyncRepository<T> where T : class
{
    protected readonly ErgoContext context;

    public BaseRepository(ErgoContext context)
    {
        this.context = context;
    }

    public virtual async Task<Result<T>> UpdateAsync(T entity)
    {
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return Result<T>.Success(entity);
    }

    public virtual async Task<Result<T>> FindByIdAsync(Guid id)
    {
        var result = await context.Set<T>().FindAsync(id);
        if (result == null)
        {
            return Result<T>.Failure($"Entity with id {id} not found");
        }

        return Result<T>.Success(result);
    }

    public virtual async Task<Result<T>> AddAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
        return Result<T>.Success(entity);
    }

    public virtual async Task<Result<T>> DeleteAsync(Guid id)
    {
        var result = await FindByIdAsync(id);
        if (result.IsSuccess)
        {
            context.Set<T>().Remove(result.Value);
            await context.SaveChangesAsync();
            return Result<T>.Success(result.Value);
        }

        return Result<T>.Failure($"Entity with id {id} not found");
    }

    public virtual async Task<Result<IReadOnlyList<T>>> GetPagedResponseAsync(int page, int size)
    {
        var result = await context.Set<T>().Skip(page).Take(size).AsNoTracking().ToListAsync();
        return Result<IReadOnlyList<T>>.Success(result);
    }
    public virtual async Task<Result<IReadOnlyList<T>>> GetAllAsync()
    {
        var result = await context.Set<T>().AsNoTracking().ToListAsync();
        return Result<IReadOnlyList<T>>.Success(result);
    }


    public virtual async Task<Result<IReadOnlyList<T>>> GetTasksByProjectIdAsync(Guid projectId)
    {
        try
        {
            var tasks = await context.Set<T>()
                .OfType<TaskItem>() // Assuming TaskItem is the entity that has ProjectId
                .Where(t => t.ProjectId == projectId)
                .AsNoTracking()
                .ToListAsync();

            IReadOnlyList<T> readOnlyTasks = tasks.Cast<T>().ToList();

            return Result<IReadOnlyList<T>>.Success(readOnlyTasks);
        }
        catch (Exception ex)
        {
            return Result<IReadOnlyList<T>>.Failure($"An error occurred while fetching tasks: {ex.Message}");
        }
    }

    public virtual async Task<Result<IReadOnlyList<T>>> GetCommentByTaskIdAsync(Guid taskId)
    {
		try
        {
			var comments = await context.Set<T>()
				.OfType<Comment>() // Assuming Comment is the entity that has TaskId
				.Where(c => c.TaskId == taskId)
				.AsNoTracking()
				.ToListAsync();

			IReadOnlyList<T> readOnlyComments = comments.Cast<T>().ToList();

			return Result<IReadOnlyList<T>>.Success(readOnlyComments);
		}
		catch (Exception ex)
        {
			return Result<IReadOnlyList<T>>.Failure($"An error occurred while fetching comments: {ex.Message}");
		}
	}

}