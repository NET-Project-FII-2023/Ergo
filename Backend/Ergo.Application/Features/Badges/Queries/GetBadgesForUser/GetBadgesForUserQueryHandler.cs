using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.Badges.Queries.GetBadgesForUser
{
    public class GetBadgesForUserQueryHandler : IRequestHandler<GetBadgesForUserQuery, GetBadgesForUserQueryResponse>
    {
        private readonly IBadgeRepository badgeRepository;
        private readonly IUserManager userManager;
        public GetBadgesForUserQueryHandler(IBadgeRepository badgeRepository, IUserManager userManager)
        {
            this.badgeRepository = badgeRepository;
            this.userManager = userManager;
        }

        public async Task<GetBadgesForUserQueryResponse> Handle(GetBadgesForUserQuery request, CancellationToken cancellationToken)
        {
            var userExists = await userManager.FindByIdAsync(request.UserId);
            if (!userExists.IsSuccess)
            {
                return new GetBadgesForUserQueryResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "User not found" }
                };
            }
            var badges = await badgeRepository.GetBadgesByUserId(request.UserId);
            if (!badges.IsSuccess)
            {
                return new GetBadgesForUserQueryResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Badges not found" }
                };
            }
            return new GetBadgesForUserQueryResponse
            {
                Success = true,
                Badges = badges.Value.Select(b => new BadgeDto
                {
                    Name = b.Name,
                    Count = b.Count,
                    Type = b.Type,
                    Active = b.Active
                }).ToArray()
            };
        }
    }

}
