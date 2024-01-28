using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ergo.Application.Responses;

namespace Ergo.Application.Features.TaskItems.Commands.UpdateTaskItem
{
    public class UpdateTaskItemCommandResponse : BaseResponse
    {
        public UpdateTaskItemCommandResponse() : base()
        {
        }
        public UpdateTaskItemDto TaskItem { get; set; }
    }
   
}
