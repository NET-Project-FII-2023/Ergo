using Microsoft.AspNetCore.Identity;

namespace Ergo.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}
