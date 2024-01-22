using Ergo.Domain.Entities.Enums;
using MediatR;

namespace Ergo.Application.Features.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommand : IRequest<UpdateProjectCommandResponse>
    {
        public Guid ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public string? Description { get; set; }
        public string? GitRepository { get; set; }
        public string ModifiedBy { get; set; } = default!;
        public DateTime Deadline { get; set; }
        public ProjectState State { get; set; }
    }
}
