using MediatR;

namespace Ergo.Application.Features.Statistics.Queries.GetTasksDueThisWeek;

public class GetTasksDueThisWeekQuery : IRequest<GetTasksDueThisWeekQueryResponse>
{
    public Guid UserId { get; set; }
}