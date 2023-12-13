namespace Ergo.App.ViewModels
{
    public class ProjectDto
    {
        public string ProjectName { get; set; }
        public string? Description { get; set; }
        public string? FullName { get; set; }
        public string? GitRepository { get; set; }
        public DateTime Deadline { get; set; }
    }
}
