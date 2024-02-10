using MediatR;

namespace Ergo.Application.Features.TaskItems.Queries.GetByUserId;

public class GetTasksByProjectsOfUsersQuery : IRequest<GetTasksByProjectsOfUsersQueryResponse>
{
    public Guid UserId { get; set; }
}