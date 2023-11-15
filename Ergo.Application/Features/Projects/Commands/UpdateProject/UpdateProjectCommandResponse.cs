using Ergo.Application.Responses;
namespace Ergo.Application.Features.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandResponse : BaseResponse
    {
        public UpdateProjectCommandResponse() : base()
        {

        }
        public UpdateProjectDto Project { get; set; }
    }
}
