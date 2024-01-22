using Ergo.Application.Responses;

namespace Ergo.Application.Features.TaskItems.Queries.GetTaskItemTime
{
    public class GetTaskItemTimeQueryResponse : BaseResponse
    {
        public GetTaskItemTimeQueryResponse() : base()
        {
        }
        public TimeSpan RecordedTime { get; set; }
    }
}