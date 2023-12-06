using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.Users.Commands.UpdateRole
{
    public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, UpdateUserRoleCommandResponse>
    {
        private readonly IUserManager userRepository;

        public UpdateUserRoleCommandHandler(IUserManager userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<UpdateUserRoleCommandResponse> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindByIdAsync(request.UserId);
            if (!user.IsSuccess)
            {
                return new UpdateUserRoleCommandResponse
                {
                    Success = false,
                    Message = user.Error
                };
            }
            var validator = new UpdateUserRoleCommandValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return new UpdateUserRoleCommandResponse
                {
                    Success = false,
                    Message = validationResult.ToString(),
                    ValidationsErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList()
                };
            }
            var result = await userRepository.UpdateRoleAsync(user.Value, request.Role);
            if (!result.IsSuccess)
            {
                return new UpdateUserRoleCommandResponse
                {
                    Success = false,
                    Message = result.Error
                };
            }
            return new UpdateUserRoleCommandResponse
            {
                Success = true
            };
        }
    }
}
