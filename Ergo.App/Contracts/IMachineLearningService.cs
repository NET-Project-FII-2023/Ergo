using Ergo.App.Services.Responses;
using Ergo.App.ViewModels;

namespace Ergo.App.Contracts
{
    public interface IMachineLearningService
    {
        Task<ApiResponse<PredictionDto>> GetPredictionAsync(TaskPredictionViewModel taskPredictionViewModel);
    }
}
