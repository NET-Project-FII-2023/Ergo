using MediatR;

namespace Ergo.Application.Features.Users.Queries.GetById
{
    public class GetByIdUserQuery : IRequest<GetByIdUserQueryResponse>
    {
        public Guid UserId { get; set; }
    }
}
