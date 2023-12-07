using Ergo.Domain.Entities.Enums;
namespace Ergo.Application.Features.Projects.Commands.UpdateProject
{
    public class UpdateProjectDto
    {
        public Guid ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public string? Description { get; set; }
        public string? GitRepository { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime Deadline { get; set; }
        public ProjectState State { get; set; }
    }
}
