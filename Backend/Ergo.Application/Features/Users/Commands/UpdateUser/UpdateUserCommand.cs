using MediatR;

namespace Ergo.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<UpdateUserCommandResponse>
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Bio { get; set; }
        public string? Mobile { get; set; }
        public string? Company { get; set; }
        public string? Location { get; set; }
        public Social? Social { get; set; }
    }
}
