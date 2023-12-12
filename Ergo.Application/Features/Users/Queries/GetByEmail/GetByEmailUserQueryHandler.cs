using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.Users.Queries.GetByEmail
{
    public class GetByEmailUserQueryHandler : IRequestHandler<GetByEmailUserQuery, GetByEmailUserQueryReponse>
    {
        private readonly IUserManager userRepository;

        public GetByEmailUserQueryHandler(IUserManager userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<GetByEmailUserQueryReponse> Handle(GetByEmailUserQuery request, CancellationToken cancellationToken)
        {
            var result = await userRepository.FindByEmailAsync(request.Email);
            if (!result.IsSuccess)
                return new GetByEmailUserQueryReponse { Success = false, Message = result.Error };
            var userDto = result.Value;
            return new GetByEmailUserQueryReponse
            {
                Success = true,
                User = new UserDto
                {
                    UserId = userDto.UserId,
                    Name = userDto.Name,
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Roles = userDto.Roles
                }
            };
        }
    }
}
