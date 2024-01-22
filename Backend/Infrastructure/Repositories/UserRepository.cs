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


}