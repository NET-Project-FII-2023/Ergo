using System.ComponentModel.DataAnnotations;

namespace Ergo.Application.Models.Identity
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Code is required")]
        public string? Code { get; set; }
    }
}
