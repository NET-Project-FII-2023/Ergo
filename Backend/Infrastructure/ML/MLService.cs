using Ergo.Application.Persistence;
using Ergo.Domain.Entities.ML;
using Microsoft.AspNetCore.Http;
using Microsoft.ML;
using static Microsoft.ML.DataOperationsCatalog;

namespace Infrastructure.ML
{
    public class MLService : IMLService
    {
        private readonly MLContext mlContext;
        private ITransformer trainedModel;

        public MLService()
        {
            mlContext = new MLContext();

        }
        public async Task TrainModelAsync()
        {
            IDataView data = mlContext.Data.LoadFromTextFile<TaskData>("I:\\Facultate\\ErgoUpdate\\Ergo\\ML\\data.csv", separatorChar: ',');

            TrainTestData splitData = mlContext.Data.TrainTestSplit(data);

            var pipeline = mlContext.Transforms.Concatenate("Features", "TaskComplexity", "NumberOfParticipants", "TaskUrgency")
                .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "ResolutionTime", maximumNumberOfIterations: 100));

            var model = pipeline.Fit(splitData.TrainSet);

            var predictions = model.Transform(splitData.TestSet);
            var metrics = mlContext.Regression.Evaluate(predictions, "ResolutionTime");
            Console.WriteLine($"R^2: {metrics.RSquared:0.##}");
            Console.WriteLine($"RMSE: {metrics.RootMeanSquaredError:0.##}");

            mlContext.Model.Save(model, data.Schema, "model.zip");
        }
        public async Task<float> PredictCompletionTime(TaskData userInput)
        {
            using (var stream = File.OpenRead("model.zip"))
            {
                trainedModel = mlContext.Model.Load(stream, out var modelInputSchema);
            }

            var predictor = mlContext.Model.CreatePredictionEngine<TaskData, TaskPrediction>(trainedModel);
            var prediction = predictor.Predict(userInput);

            Console.WriteLine($"Predicted resolution time: {prediction.PredictedResolutionTime}");
            return prediction.PredictedResolutionTime;
        }
    }
}
