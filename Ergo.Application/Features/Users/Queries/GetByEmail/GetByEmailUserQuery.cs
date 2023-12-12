using MediatR;

namespace Ergo.Application.Features.Users.Queries.GetByEmail
{
    public class GetByEmailUserQuery : IRequest<GetByEmailUserQueryReponse>
    {
        public string Email { get; set; }
    }
}
