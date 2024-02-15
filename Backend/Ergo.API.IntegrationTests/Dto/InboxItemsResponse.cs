using Ergo.Application.Features.InboxItems.Queries.GetByUserId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.API.IntegrationTests.Dto
{
    public class InboxItemsResponse
    {
        public List<GetByUserIdDto> InboxItems { get; set; }
    }
}
