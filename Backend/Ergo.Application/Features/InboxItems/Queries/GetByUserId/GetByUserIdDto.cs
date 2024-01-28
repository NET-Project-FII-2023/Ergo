using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Features.InboxItems.Queries.GetByUserId
{
    public class GetByUserIdDto
    {
        public Guid InboxItemId { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsRead { get; set; }
    }
}
