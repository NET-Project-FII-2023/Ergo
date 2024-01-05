using Ergo.Application.Persistence;
using Ergo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    internal class InboxItemRepository : BaseRepository<InboxItem>, IInboxItemRepository
    {
        public InboxItemRepository(ErgoContext context) : base(context)
        { 
        }
    }
}
