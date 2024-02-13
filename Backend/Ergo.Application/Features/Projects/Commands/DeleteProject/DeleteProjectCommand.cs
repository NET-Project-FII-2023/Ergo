using MediatR;

namespace Ergo.Application.Features.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommand : IRequest<DeleteProjectCommandResponse>
    {
        public Guid ProjectId { get; set; }
        public string Owner { get; set; }

    }
}
