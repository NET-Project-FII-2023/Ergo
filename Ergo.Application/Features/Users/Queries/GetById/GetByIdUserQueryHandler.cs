using Ergo.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Features.Users.Queries.GetById
{
    public class GetByIdUserQueryHandler : IRequestHandler<GetByIdUserQuery, UserDto>

    {
        private readonly IUserRepository userRepository;
        public GetByIdUserQueryHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<UserDto> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindByIdAsync(request.UserId);
            if(user.IsSuccess)
            {
                return new UserDto
                {
                    UserId = user.Value.UserId,
                    FirstName = user.Value.FirstName,
                    LastName = user.Value.LastName,
                    Email = user.Value.Email,
                    Password = user.Value.Password,
                    Role = user.Value.Role
                };
            }
            return new UserDto();
        }
    }
}
