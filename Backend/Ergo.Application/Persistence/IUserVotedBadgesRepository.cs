using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Persistence
{
    public interface IUserVotedBadgesRepository : IAsyncRepository<UserVotedBadges>
    {
        Task<bool> GetUserVotedBadge(Guid voterId, Guid votedId, string type);
    }
}
