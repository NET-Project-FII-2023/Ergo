using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Features.Users.Queries.GetById
{
    public class GetByIdUserQueryHandler : IRequestHandler<GetByIdUserQuery, GetByIdUserQueryResponse>

    {
        private readonly IUserRepository userRepository;
        public GetByIdUserQueryHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<GetByIdUserQueryResponse> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
        {
            GetByIdUserQueryResponse response = new();
            var user = await userRepository.FindByIdAsync(request.UserId);
            if(!user.IsSuccess)
            {
                response.Success = false;
                response.ValidationsErrors = new List<string> { user.Error };
                return response;
            }

                return new GetByIdUserQueryResponse
                {
                    Success = true,
                    User =  new UserDto
                    {
                        UserId = user.Value.UserId,
                        FirstName = user.Value.FirstName,
                        LastName = user.Value.LastName,
                        Email = user.Value.Email,
                        Role = user.Value.Role
                    } 

                };
        }
    }
}
