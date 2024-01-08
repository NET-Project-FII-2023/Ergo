using Ergo.Application.Features.InboxItems.Commands.CreateInboxItem;
using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.InboxItems.Queries.GetByUserId
{
    public class GetByUserIdQueryHandler : IRequestHandler<GetByUserIdQuery, GetByUserIdQueryResponse>
    {
        private readonly IInboxItemRepository inboxItemRepository;
        private readonly IUserRepository userRepository;

        public GetByUserIdQueryHandler(IInboxItemRepository inboxItemRepository, IUserRepository userRepository)
        {
            this.inboxItemRepository = inboxItemRepository;
            this.userRepository = userRepository;
        }

        public async Task<GetByUserIdQueryResponse> Handle(GetByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userExists = await userRepository.FindByIdAsync(request.UserId);
            if(!userExists.IsSuccess)
            {
                return new GetByUserIdQueryResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "User with the provided ID does not exist." }
                };
            }
            var inboxItems = await inboxItemRepository.GetByUserIdAsync(request.UserId);
            return new GetByUserIdQueryResponse
            {
                Success = true,
                InboxItems = inboxItems.Select(x => new GetByUserIdDto
                {
                   InboxItemId = x.InboxItemId,
                   UserId = x.UserId,
                   Message = x.Message,
                   CreatedDate = x.CreatedDate,
                   IsRead = x.IsRead,

                }).ToList()
            };
               
        }
    }
}
