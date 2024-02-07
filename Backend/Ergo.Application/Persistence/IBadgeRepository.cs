using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Persistence
{
    public interface IBadgeRepository : IAsyncRepository<Badge>
    {
        Task<Result<Badge>> GetBadgeByUserIdAndType(Guid userId, string badgeType);
        Task<Result<List<Badge>>> GetBadgesByUserId(Guid userId);
    }
}
