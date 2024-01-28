using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Features.TaskItems.Queries.GetAll
{
    public class GetAllTaskItemsResponse
    {
        public List<TaskItemDto> TaskItems { get; set; }
    }
}
