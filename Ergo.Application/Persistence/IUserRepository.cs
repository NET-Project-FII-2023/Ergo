using Ergo.Domain.Common;
using Ergo.Domain.Entities;

namespace Ergo.Application.Persistence;

public interface IUserRepository : IAsyncRepository<User>
{
}