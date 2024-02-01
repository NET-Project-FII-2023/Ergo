using Ergo.Application.Responses;

namespace Ergo.Application.Features.Projects.Queries.GetProjectGithubData
{
    public class GetProjectGithubDataQueryResponse : BaseResponse
    {
        public GetProjectGithubDataQueryResponse() : base()
        {
        }
        public string ProjectOwner { get; set; }
        public string ProjectRepository { get; set; }
        public string GithubToken { get; set; }
    }
}