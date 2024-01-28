using Microsoft.ML.Data;

namespace Ergo.Domain.Entities.ML
{
    public class TaskPrediction
    {
        [ColumnName("Score")]
        public float PredictedResolutionTime { get; set; }
    }
}
