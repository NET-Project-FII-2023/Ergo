using MediatR;

namespace Ergo.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<DeleteUserCommandResponse>
    {
        public Guid UserId { get; set; }
    }
}
