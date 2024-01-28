using Ergo.Application.Responses;

namespace Ergo.Application.Features.Projects.Queries.GetProjectsByUserId
{
    public class GetProjectsByUserIdQueryResponse : BaseResponse
    {
        public GetProjectsByUserIdQueryResponse() : base()
        {
            
        }
        public List<ProjectDto> Projects { get; set; }
    }
}