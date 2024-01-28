using Ergo.Application.Responses;

namespace Ergo.Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectCommandResponse : BaseResponse
    {
        public CreateProjectCommandResponse() : base()
        {
        }
        public CreateProjectDto Project { get; set; }
    }
}

