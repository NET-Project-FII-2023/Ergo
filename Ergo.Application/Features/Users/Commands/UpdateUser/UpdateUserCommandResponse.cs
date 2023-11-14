using Ergo.Application.Responses;

namespace Ergo.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandResponse : BaseResponse
    {
        public UpdateUserCommandResponse() : base()
        {
        }
        public UpdateUserDto User { get; set; }
    }
}