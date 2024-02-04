using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.Users.Queries.GetById
{
    public class GetByIdUserQueryHandler : IRequestHandler<GetByIdUserQuery, GetByIdUserQueryResponse>

    {
        private readonly IUserManager userRepository;
        public GetByIdUserQueryHandler(IUserManager userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<GetByIdUserQueryResponse> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
        {
            var result = await userRepository.FindByIdAsync(Guid.Parse(request.UserId));
            if (!result.IsSuccess)
                return new GetByIdUserQueryResponse { Success = false, Message = result.Error };
            var userDto = result.Value;
            return new GetByIdUserQueryResponse
            {
                Success = true,
                User = new UserDto
                {
                    UserId = userDto.UserId,
                    Name = userDto.Name,
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Bio = userDto.Bio,
                    Mobile = userDto.Mobile,
                    Company = userDto.Company,
                    Location = userDto.Location,
                    Social = userDto.Social,
                    Roles = userDto.Roles,
                }
            };
            
        }
    }
}
