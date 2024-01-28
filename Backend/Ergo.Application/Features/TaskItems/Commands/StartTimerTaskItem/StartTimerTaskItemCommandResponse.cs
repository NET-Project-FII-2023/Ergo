using Ergo.Application.Responses;

namespace Ergo.Application.Features.TaskItems.Commands.StartTimerTaskItem
{
    public class StartTimerTaskItemCommandResponse : BaseResponse
    {
        public StartTimerTaskItemCommandResponse() : base()
        {

        }
        public DateTime StartTime { get; set; }
    }
}