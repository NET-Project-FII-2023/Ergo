using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeleteUserCommandResponse>
    {
        private readonly IUserManager userRepository;

        public DeleteUserCommandHandler(IUserManager userRepository) {
            this.userRepository = userRepository;
        }

        public async Task<DeleteUserCommandResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var result = await userRepository.DeleteAsync(request.UserId);
            if (!result.IsSuccess)
                return new DeleteUserCommandResponse { Success = false, Message = result.Error };
            return new DeleteUserCommandResponse { Success = true };
        }
    }
}
