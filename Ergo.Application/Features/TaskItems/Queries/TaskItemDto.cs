using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ergo.Domain.Entities.Enums;

namespace Ergo.Application.Features.TaskItems.Queries
{
    public class TaskItemDto
    {
        public string? TaskName { get; set; }
        public string? Description { get; set; }
        public DateTime Deadline { get; set; }
        public string? CreatedBy { get; set; }
        public Guid ProjectId { get; set; }

        public TaskState State { get; set; }
    }
}
