using MediatR;

namespace Ergo.Application.Features.Projects.Queries.GetById
{
    public record GetByIdProjectQuery(Guid ProjectId) : IRequest<ProjectDto>;
}
