using Ergo.Application.Persistence;
using Ergo.Domain.Entities;
using MediatR;

namespace Ergo.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserCommandResponse>
    {
        private readonly IUserRepository userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindByIdAsync(request.UserId);
            if (!user.IsSuccess)
            {
                return new UpdateUserCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "User with id this does not exists" }
                };
            }
            request.FirstName ??= user.Value.FirstName;
            request.LastName ??= user.Value.LastName;
            request.Email ??= user.Value.Email;
            request.Password ??= user.Value.Password;
            request.Role ??= user.Value.Role;

            var validator = new UpdateUserCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            { 
                return new UpdateUserCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList()
                };
            }
            user.Value.UpdateData(request.FirstName, request.LastName, request.Email, request.Password, request.Role.Value);


            var result = await userRepository.UpdateAsync(user.Value);
            return new UpdateUserCommandResponse
            {
                Success = true,
                User = new UpdateUserDto
                {
                    FirstName = result.Value.FirstName,
                    LastName = result.Value.LastName,
                    Email = result.Value.Email,
                    Password = result.Value.Password,
                    Role = result.Value.Role
                }
            };

        }
    }
}
