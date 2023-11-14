using Ergo.Application.Responses;

namespace Ergo.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandResponse : BaseResponse
    {
        public CreateUserCommandResponse():base()
        {   
        }
        public CreateUserDto User { get; set; }
    }
}
