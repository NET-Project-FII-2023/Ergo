using MediatR;

namespace Ergo.Application.Features.Projects.Commands.AssignUserToProject
{
    public class AssignUserToProjectCommand : IRequest<AssignUserToProjectCommandResponse>
    {
        public string UserId { get; set; }
        public string ProjectId { get; set; }
    }
}
