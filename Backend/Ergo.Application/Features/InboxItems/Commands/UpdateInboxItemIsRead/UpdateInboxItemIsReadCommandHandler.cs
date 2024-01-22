using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.InboxItems.Commands.UpdateInboxItemIsRead
{
    public class UpdateInboxItemIsReadCommandHandler: IRequestHandler<UpdateInboxItemIsReadCommand, UpdateInboxItemIsReadCommandResponse>
    {
        private readonly IInboxItemRepository inboxItemRepository;

        public UpdateInboxItemIsReadCommandHandler(IInboxItemRepository inboxItemRepository)
        {
            this.inboxItemRepository = inboxItemRepository;
        }
        public async Task<UpdateInboxItemIsReadCommandResponse> Handle(UpdateInboxItemIsReadCommand request, CancellationToken cancellationToken)
        {
            var inboxItem = await inboxItemRepository.FindByIdAsync(request.InboxItemId);
            if (!inboxItem.IsSuccess)
            {
                return new UpdateInboxItemIsReadCommandResponse
                {
                    Success = false,
                    Message = inboxItem.Error
                };
            }
            var result = await inboxItemRepository.UpdateIsReadAsync(inboxItem.Value, true);
            if (!result.IsSuccess)
            {
                return new UpdateInboxItemIsReadCommandResponse
                {
                    Success = false,
                    Message = result.Error
                };
            }
            return new UpdateInboxItemIsReadCommandResponse
            {
                Success = true
            };

        }

    }
}
