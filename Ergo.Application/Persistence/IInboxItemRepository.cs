using Ergo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Persistence
{
    public interface IInboxItemRepository : IAsyncRepository<InboxItem>
    {
    }
}
