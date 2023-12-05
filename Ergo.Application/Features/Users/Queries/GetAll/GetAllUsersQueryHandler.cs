using Ergo.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Features.Users.Queries.GetAll
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, GetAllUsersResponse>
    {
        private readonly IUserManager userRepository;
        public GetAllUsersQueryHandler(IUserManager userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<GetAllUsersResponse> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            GetAllUsersResponse response = new();
            var result = await userRepository.GetAllAsync();
            if(result.IsSuccess)
            {
                response.Users = result.Value.Select(u => new UserDto
                { 
                    UserId = u.UserId,
                    Name = u.Name,
                    Username = u.Username,
                    Email = u.Email,
                    Password = u.Password,
                }).ToList();
            }
            return response;
        }
    }
}
