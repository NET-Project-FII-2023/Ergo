using MediatR;

namespace Ergo.Application.Features.Users.Commands.UpdateRole
{
    public class UpdateUserRoleCommand : IRequest<UpdateUserRoleCommandResponse>
    {
        public Guid UserId { get; set; }
        public string Role { get; set; }
    }
}
