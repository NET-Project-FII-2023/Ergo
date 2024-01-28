using Ergo.Application.Responses;

namespace Ergo.Application.Features.Users.Queries.GetByEmail
{
    public class GetByEmailUserQueryReponse : BaseResponse
    {
        public GetByEmailUserQueryReponse() : base()
        {
            
        }
        public UserDto User { get; set; }
    }
}