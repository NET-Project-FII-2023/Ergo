using Ergo.Application.Features.Users.Queries;
using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserCommandResponse>
    {
        private readonly IUserManager userRepository;

        public UpdateUserCommandHandler(IUserManager userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateUserCommandResponse();

            var user = await userRepository.FindByIdAsync(request.Id);
            if(!user.IsSuccess)
            {
                return new UpdateUserCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "User with id this does not exists" }
                };
            }
            request.Name ??= user.Value.Name;
            request.Username ??= user.Value.Username;
            request.Email ??= user.Value.Email;
            var validator = new UpdateUserCommandValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return new UpdateUserCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList()
                };
            }
            UserDto userDto = new()
            {
                UserId = user.Value.UserId,
                Name = request.Name,
                Username = request.Username,
                Email = request.Email,
            };
            var result = await userRepository.UpdateAsync(userDto);
            if (!result.IsSuccess)
            {
                return new UpdateUserCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { result.Error }
                };
            }
            return new UpdateUserCommandResponse
            {
                Success = true,
                User = new UpdateUserDto
                {
                    Name = result.Value.Name,
                    Username = result.Value.Username,
                    Email = result.Value.Email,
                }
            };
        }
    }
}
