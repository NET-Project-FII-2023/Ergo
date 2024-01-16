using Microsoft.ML.Data;

namespace Ergo.App.ViewModels
{
    public class TaskPredictionViewModel
    { 
        public float TaskComplexity { get; set; }
        public float NumberOfParticipants { get; set; }
        public float TaskUrgency { get; set; }
    }
}
