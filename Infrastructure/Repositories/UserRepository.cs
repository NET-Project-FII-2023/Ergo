using Ergo.Domain.Entities;
using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ErgoContext context) : base(context)
    {

    }

    public async Task<Result<User>> FindUserByEmailAsync(string email)
    {
        var result = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (result == null)
        {
            return Result<User>.Failure($"User with email {email} not found");
        }

        return Result<User>.Success(result);
    }
}