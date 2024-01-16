using Ergo.Domain.Entities.ML;
using MediatR;

namespace Ergo.Application.Features.ML.Query.GetTaskPrediction
{
    public class TaskPredictionQuery : IRequest<TaskPredictionQueryResponse>
    {
        public TaskDataDto TaskData { get; set; }

    }
}
