using Ergo.Application.Responses;

namespace Ergo.Application.Features.ML.Query.GetTaskPrediction
{
    public class TaskPredictionQueryResponse : BaseResponse
    {
        public TaskPredictionQueryResponse() : base()
        { }
        public float PredictedResolutionTime { get; set; }
    }
}