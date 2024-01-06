using Ergo.Application.Features.InboxItems.Commands.CreateInboxItem;
using Ergo.Application.Responses;

namespace Ergo.Application.Features.InboxItems.Queries.GetByUserId
{
    public class GetByUserIdQueryResponse : BaseResponse
    {
        public GetByUserIdQueryResponse() : base()
        {

        }
        public List<GetByUserIdDto> InboxItems { get; set;}
    }
}