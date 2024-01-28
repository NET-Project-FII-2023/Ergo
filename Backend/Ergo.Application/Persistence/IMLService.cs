using Ergo.Domain.Common;
using Ergo.Domain.Entities.ML;
using Microsoft.AspNetCore.Http;

namespace Ergo.Application.Persistence
{
    public interface IMLService
    {
        Task TrainModelAsync();
        Task<float> PredictCompletionTime(TaskData userInput);

    }
}
