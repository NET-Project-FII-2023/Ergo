namespace Ergo.App.ViewModels
{
    public class RemoveUserFromProjectDto
    {
        public string OwnerUsername { get; set; }
        public string ProjectId { get; set; }
        public string UserId { get; set; }
    }
}
