using Ergo.Application.Persistence;
using Ergo.Domain.Entities;
using Ergo.Domain.Entities.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
    {
        private readonly IUserRepository userRepository;
        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<CreateUserCommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateUserCommandValidator();
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validatorResult.IsValid)
            {
                return new CreateUserCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }
            var user = User.Create(request.FirstName, request.LastName, request.Email, request.Password, request.Role);
            if (!user.IsSuccess)
            {
                return new CreateUserCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { user.Error }
                };
            }
            await userRepository.AddAsync(user.Value);
            return new CreateUserCommandResponse
            {
                Success = true,
                User = new CreateUserDto
                {
                    UserId = user.Value.UserId,
                    FirstName = user.Value.FirstName,
                    LastName = user.Value.LastName,
                    Email = user.Value.Email,
                    Password = user.Value.Password,
                    Role = user.Value.Role
                }
            };

        }
    }
}

