using Ergo.Domain.Entities;
using Ergo.Application.Persistence;
namespace Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ErgoContext context) : base(context)
    {
    }
}