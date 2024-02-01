using MediatR;

namespace Ergo.Application.Features.Projects.Queries.GetProjectGithubBranches
{
    public class GetProjectGithubBranchesQuery : IRequest<GetProjectGithubBranchesQueryResponse>
    {
        public Guid ProjectId { get; set; }

    }
}
