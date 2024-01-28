using Microsoft.ML.Data;

namespace Ergo.Domain.Entities.ML
{
    public class TaskData
    {
        [LoadColumn(0)]
        public float TaskComplexity { get; set; }
        [LoadColumn(1)]
        public float NumberOfParticipants { get; set; }
        [LoadColumn(2)]
        public float TaskUrgency { get; set; }
        [LoadColumn(3)]
        public float ResolutionTime { get; set; }

    }
}
