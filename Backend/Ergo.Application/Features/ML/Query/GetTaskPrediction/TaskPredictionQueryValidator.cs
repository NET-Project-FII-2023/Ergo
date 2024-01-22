using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Features.ML.Query.GetTaskPrediction
{
    public class TaskPredictionQueryValidator : AbstractValidator<TaskPredictionQuery>
    {
        public TaskPredictionQueryValidator()
        {
            RuleFor(x => x.TaskData.NumberOfParticipants).InclusiveBetween(1, 50).WithMessage("Number of participants should be between 1 and 50");
            RuleFor(x => x.TaskData.TaskComplexity).InclusiveBetween(1, 3).WithMessage("Task complexity should be between 1 and 3");
            RuleFor(x => x.TaskData.TaskUrgency).InclusiveBetween(1, 3).WithMessage("Task urgency should be between 1 and 3");
        }

    }
}
