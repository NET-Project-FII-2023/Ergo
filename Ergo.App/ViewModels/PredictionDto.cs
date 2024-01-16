namespace Ergo.App.ViewModels
{
    public class PredictionDto
    {
        public float TaskComplexity { get; set; }
        public float NumberOfParticipants { get; set; }
        public float TaskUrgency { get; set; }

        public float PredictedResolutionTime { get; set; }
    }
}
