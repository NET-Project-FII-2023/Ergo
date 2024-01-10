using MediatR;

namespace Ergo.Application.Features.Projects.Commands.DeleteUserFromProject
{
    public class DeleteUserFromProjectCommand : IRequest<DeleteUserFromProjectCommandResponse>
    {
        public string OwnerUsername { get; set; }
        public string UserId { get; set; }
        public string ProjectId { get; set; }
    }
}