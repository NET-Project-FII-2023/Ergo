using Ergo.Application.Persistence;
using Ergo.Domain.Entities.ML;
using MediatR;

namespace Ergo.Application.Features.ML.Query.GetTaskPrediction
{
    public class TaskPredictionQueryHandler : IRequestHandler<TaskPredictionQuery, TaskPredictionQueryResponse>
    {
        private readonly IMLService mlService;
        public TaskPredictionQueryHandler(IMLService mlService)
        {
            this.mlService = mlService;
        }

        public async Task<TaskPredictionQueryResponse> Handle(TaskPredictionQuery request, CancellationToken cancellationToken)
        {
            var validationResults = new TaskPredictionQueryValidator().Validate(request);
            if (!validationResults.IsValid)
            {
                return new TaskPredictionQueryResponse { Success = false, ValidationsErrors = validationResults.Errors.Select(x => x.ErrorMessage).ToList() };
            }
            var taskData = new TaskData { NumberOfParticipants = request.TaskData.NumberOfParticipants, TaskComplexity = request.TaskData.TaskComplexity, TaskUrgency = request.TaskData.TaskUrgency };
            var result = await mlService.PredictCompletionTime(taskData);
            return new TaskPredictionQueryResponse { PredictedResolutionTime = result };
        }
    }
}
