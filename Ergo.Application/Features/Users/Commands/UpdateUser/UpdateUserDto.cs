using Ergo.Domain.Entities.Enums;

namespace Ergo.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public UserRole Role { get; set; }
    }
}