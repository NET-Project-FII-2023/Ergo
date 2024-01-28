using Ergo.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Features.TaskItems.Commands.CreateTaskItem
{
    public class CreateTaskItemCommandResponse : BaseResponse
    {
        public CreateTaskItemCommandResponse() : base()
        {
        }
        public CreateTaskItemDto TaskItem { get; set;}
    }
}
