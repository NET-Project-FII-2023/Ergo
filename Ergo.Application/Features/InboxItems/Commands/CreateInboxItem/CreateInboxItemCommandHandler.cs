using Ergo.Application.Persistence;
using Ergo.Domain.Entities;
using MediatR;

namespace Ergo.Application.Features.InboxItems.Commands.CreateInboxItem
{
    public class CreateInboxItemCommandHandler : IRequestHandler<CreateInboxItemCommand, CreateInboxItemCommandResponse>
    {
        private readonly IInboxItemRepository inboxItemRepository;
        private readonly IUserRepository userRepository;

        public CreateInboxItemCommandHandler(IInboxItemRepository inboxItemRepository, IUserRepository userRepository)
        {
            this.inboxItemRepository = inboxItemRepository;
            this.userRepository = userRepository;
        }

        public async Task<CreateInboxItemCommandResponse> Handle(CreateInboxItemCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateInboxItemCommandValidator();
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validatorResult.IsValid)
            {
                return new CreateInboxItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }
            var userExists = await userRepository.FindByIdAsync(request.UserId);
            if (!userExists.IsSuccess)
            {
                return new CreateInboxItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "User with the provided ID does not exist." }
                };
            }
            var inboxItem = InboxItem.Create(request.UserId, request.Message);
            if (!inboxItem.IsSuccess)
            {
                return new CreateInboxItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { inboxItem.Error }
                };
            }
            await inboxItemRepository.AddAsync(inboxItem.Value);
            return new CreateInboxItemCommandResponse
            {
                Success = true,
                InboxItem = new CreateInboxItemDto
                {
                    Message = inboxItem.Value.Message,
                    UserId = inboxItem.Value.UserId,
                    CreatedDate = inboxItem.Value.CreatedDate,
                    IsRead = inboxItem.Value.IsRead
                }
            };
        }
    }
}
