using System.ComponentModel.DataAnnotations;

namespace Ergo.App.ViewModels
{
    public class UserViewModel
    {
        public string? UserId { get; set; }

        public string? Username { get; set; }

        public string? Name { get; set; }
        public string? Email { get; set; }
        public List<string>? Roles { get; set; }
    }
}