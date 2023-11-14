using Ergo.Domain.Entities;
using Ergo.Domain.Entities.Enums;

namespace Ergo.Application.Features.Projects.Queries
{
    public class ProjectDto
    {
        public Guid ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Deadline { get; set; }
        public ProjectState State { get; set; }
        public List<User>? Members { get; set; }
    }
}
