using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Features.TaskItems.Commands.UpdateTaskItem
{
    public class UpdateTaskItemDto
    {
        public Guid TaskItemId { get; set; }
        public string? TaskName { get; set; }
        public string? Description { get; set; }
        public DateTime Deadline { get; set; }
        public string? CreatedBy { get; set; }
        public Guid ProjectId { get; set; }
    }
}
