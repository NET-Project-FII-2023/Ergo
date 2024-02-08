using Ergo.Application.Persistence;
using Ergo.Domain.Entities;
using MediatR;

namespace Ergo.Application.Features.Badges.Commands.CreateBadgeCommand
{
    public class CreateBadgeCommandHandler : IRequestHandler<CreateBadgeCommand, CreateBadgeCommandResponse>
    {
        private readonly IBadgeRepository badgeRepository;
        private readonly IUserManager userManager;
        public CreateBadgeCommandHandler(IBadgeRepository badgeRepository, IUserManager userManager)
        {
            this.badgeRepository = badgeRepository;
            this.userManager = userManager;
        }

        public async Task<CreateBadgeCommandResponse> Handle(CreateBadgeCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateBadgeCommandValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return new CreateBadgeCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validationResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }
            var userExists = await userManager.FindByIdAsync(request.UserId);
            if (!userExists.IsSuccess)
            {
                return new CreateBadgeCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "User not found" }
                };
            }

            var badgesByUserIdAndType = await badgeRepository.GetBadgeByUserIdAndType(request.UserId, request.Type);
            if (badgesByUserIdAndType.IsSuccess)
            {
                var badge = badgesByUserIdAndType.Value;
                if (badge.Count > request.Count)
                {
                    return new CreateBadgeCommandResponse
                    {
                        Success = true,
                    };
                }

                badge.UpdateCount(request.Count);
                await badgeRepository.UpdateAsync(badge);
                return new CreateBadgeCommandResponse
                {
                    Success = true,
                };
            }
            var newBadge = Badge.Create(request.Name, request.Count, request.UserId, request.Type);
            if (!newBadge.IsSuccess)
            {
                return new CreateBadgeCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { newBadge.Error }
                };
            }
            await badgeRepository.AddAsync(newBadge.Value);
            return new CreateBadgeCommandResponse
            {
                Success = true,
            };

        }
    }

}
