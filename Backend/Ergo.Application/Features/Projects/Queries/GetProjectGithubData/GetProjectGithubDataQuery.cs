using MediatR;

namespace Ergo.Application.Features.Projects.Queries.GetProjectGithubData
{
    public class GetProjectGithubDataQuery : IRequest<GetProjectGithubDataQueryResponse>
    {
        public Guid ProjectId { get; set; }
        public string Branch { get; set; }
    }
}
