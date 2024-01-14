using Ergo.Application.Responses;

namespace Ergo.Application.Features.TaskItems.Commands.PauseTimerTaskItem
{
    public class PauseTimerTaskItemCommandResponse : BaseResponse
    {
        public PauseTimerTaskItemCommandResponse() : base()
        {

        }
        public TimeSpan TotalTimeSpent { get; set; }
    }
}