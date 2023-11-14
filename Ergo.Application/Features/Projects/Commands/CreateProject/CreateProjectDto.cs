namespace Ergo.Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectDto
    {
        public Guid ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public string? Description { get; set; }
        public string? FullName { get; set; }
        public DateTime Deadline { get; set; }
    }
}
