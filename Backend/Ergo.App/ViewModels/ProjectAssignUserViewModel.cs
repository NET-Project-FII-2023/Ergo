using System.ComponentModel.DataAnnotations;

namespace Ergo.App.ViewModels
{
    public class ProjectAssignUserViewModel
    {
        [Required(ErrorMessage = "User email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        public string ProjectId { get; set;}
        public string UserId { get; set;}
    }
}
