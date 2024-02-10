using Ergo.Application.Features.Badges.Commands.CreateBadgeCommand;
using Ergo.Application.Persistence;
using Ergo.Domain.Entities;
using MediatR;

namespace Ergo.Application.Features.Badges.Commands.UpdateSpecialBadgeCommand
{
    public class UpdateSpecialBadgeCommandHandler : IRequestHandler<UpdateSpecialBadgeCommand, UpdateSpecialBadgeCommandResponse>
    {
        private readonly IBadgeRepository badgeRepository;
        private readonly IUserManager userManager;
        private readonly IUserVotedBadgesRepository userVotedBadgesRepository;
        public UpdateSpecialBadgeCommandHandler(IBadgeRepository badgeRepository, IUserManager userManager, IUserVotedBadgesRepository userVotedBadgesRepository)
        {
            this.badgeRepository = badgeRepository;
            this.userManager = userManager;
            this.userVotedBadgesRepository = userVotedBadgesRepository;
        }

        public async Task<UpdateSpecialBadgeCommandResponse> Handle(UpdateSpecialBadgeCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateSpecialBadgeCommandValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return new UpdateSpecialBadgeCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validationResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }
            var voterExist = await userManager.FindByIdAsync(request.VoterId);
            if (!voterExist.IsSuccess)
            {
                return ReturnBadResponse("Voter not found");
            }
            var votedExist = await userManager.FindByIdAsync(request.VotedId);
            if (!votedExist.IsSuccess)
            {
                return ReturnBadResponse("Voted not found");
            }
            var badge = await badgeRepository.GetBadgeByUserIdAndType(request.VotedId, request.Type);
            if (!badge.IsSuccess)
            {
                return ReturnBadResponse("Badge not found");
            }
            var userVotedAlready = await userVotedBadgesRepository.GetUserVotedBadge(request.VoterId, request.VotedId, request.Type);
            if (userVotedAlready)
            {
                return ReturnBadResponse("You have already endorsed this user with this badge");
            }
            var userVotedBadge = UserVotedBadges.Create(request.VoterId, request.VotedId, request.Type);
            var resultUserVoted = await userVotedBadgesRepository.AddAsync(userVotedBadge.Value);
            if (!resultUserVoted.IsSuccess)
            {
               return ReturnBadResponse("Error adding user voted badge");
            }
            badge.Value.UpdateCount(badge.Value.Count + 1);
            var result = await badgeRepository.UpdateAsync(badge.Value);
            if (!result.IsSuccess)
            {
                return ReturnBadResponse("Error updating badge");

            }
            return new UpdateSpecialBadgeCommandResponse
            {
                Success = true,
            };


        }
        public UpdateSpecialBadgeCommandResponse ReturnBadResponse(string error)
        {
            return new UpdateSpecialBadgeCommandResponse
            {
                Success = false,
                ValidationsErrors = new List<string> { error }
            };
        }
    }

}
