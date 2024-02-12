using Ergo.Application.Models.Statistics;
using Ergo.Application.Responses;
using Ergo.Domain.Entities;

namespace Ergo.Application.Features.Statistics.Queries.GetTasksDueThisWeek;

public class GetTasksDueThisWeekQueryResponse : BaseResponse
{
    public List<TaskDueModel> TasksDueThisWeek { get; set; }
}