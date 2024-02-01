using Ergo.Domain.Entities.Enums;
using MediatR;

namespace Ergo.Application.Features.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommand : IRequest<UpdateProjectCommandResponse>
    {
        public string? ProjectOwner { get; set; }
        public Guid ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public string? Description { get; set; }
        public string? GithubToken { get; set; }
        public string? GitRepository { get; set; }
        public DateTime Deadline { get; set; }
        public ProjectState State { get; set; }
    }
}
