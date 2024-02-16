using Ergo.Application.Features.Badges.Queries.GetBadgesForUser;
using Ergo.Application.Features.Users.Queries;
using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using FluentAssertions;
using NSubstitute;
namespace Ergo.Application.Tests.Badges.Queries
{
    public class GetBadgesForUserQueryHandlerTests
    {
        private readonly IUserManager _userManager = Substitute.For<IUserManager>();
        private readonly IBadgeRepository _badgeRepository = Substitute.For<IBadgeRepository>();
        private readonly GetBadgesForUserQueryHandler _handler;

        public GetBadgesForUserQueryHandlerTests()
        {
            _handler = new GetBadgesForUserQueryHandler(_badgeRepository, _userManager);
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var query = new GetBadgesForUserQuery { UserId = Guid.NewGuid() };
            _userManager.FindByIdAsync(query.UserId).Returns(Task.FromResult(Result<UserDto>.Failure("User not found")));

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            response.Success.Should().BeFalse();
            response.ValidationsErrors.Should().Contain("User not found");
        }

        [Fact]
        public async Task Handle_BadgesNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var query = new GetBadgesForUserQuery { UserId = Guid.NewGuid() };
            _userManager.FindByIdAsync(query.UserId).Returns(Task.FromResult(Result<UserDto>.Success(new UserDto())));
            _badgeRepository.GetBadgesByUserId(query.UserId).Returns(Task.FromResult(Result<List<Badge>>.Failure("Badges not found")));

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            response.Success.Should().BeFalse();
            response.ValidationsErrors.Should().Contain("Badges not found");
        }

        [Fact]
        public async Task Handle_SuccessfulRetrieval_ReturnsBadges()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var badges = new List<Badge>
            {
                new Badge { Name = "Test Badge", Count = 1, Type = "Innovator", Active = true }
            };
            var query = new GetBadgesForUserQuery { UserId = userId };
            _userManager.FindByIdAsync(userId).Returns(Task.FromResult(Result<UserDto>.Success(new UserDto())));
            _badgeRepository.GetBadgesByUserId(userId).Returns(Task.FromResult(Result<List<Badge>>.Success(badges)));

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            response.Success.Should().BeTrue();
            response.Badges.Length.Should().Be(badges.Count);
            response.Badges[0].Name.Should().Be(badges[0].Name);
        }

    }

}
