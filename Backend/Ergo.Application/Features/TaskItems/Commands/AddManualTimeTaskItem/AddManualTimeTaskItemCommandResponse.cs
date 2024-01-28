using Ergo.Application.Responses;

namespace Ergo.Application.Features.TaskItems.Commands.AddManualTimeTaskItem
{
    public class AddManualTimeTaskItemCommandResponse : BaseResponse
    {
        public AddManualTimeTaskItemCommandResponse() : base()
        {

        }
        public TimeSpan TotalTimeSpent { get; set; }
    }
}