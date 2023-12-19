using Ergo.Domain.Entities;
using Ergo.Domain.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace Ergo.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }

        public void UpdateData(string name, string email)
        {
            Name = name;
            Email = email;
        }

    }
}
