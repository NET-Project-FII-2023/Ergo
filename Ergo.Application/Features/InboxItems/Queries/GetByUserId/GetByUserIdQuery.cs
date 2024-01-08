using Ergo.Application.Responses;
using MediatR;

namespace Ergo.Application.Features.InboxItems.Queries.GetByUserId
{
    public class GetByUserIdQuery : IRequest<GetByUserIdQueryResponse>
    {
        public Guid UserId { get; set; }
    }

}
