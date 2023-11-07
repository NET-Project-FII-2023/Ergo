using Ergo.Application.Persistence;
using Ergo.Domain.Entities;

namespace Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ErgoContext context) : base(context)
    {
    }
}