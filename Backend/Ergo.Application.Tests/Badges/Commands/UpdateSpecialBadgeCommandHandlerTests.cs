using Ergo.Application.Features.Badges.Commands.UpdateSpecialBadgeCommand;
using Ergo.Application.Features.Users.Queries;
using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using FluentAssertions;
using NSubstitute;
namespace Ergo.Application.Tests.Badges.Commands
{
    public class UpdateSpecialBadgeCommandHandlerTests
    {
        private readonly IBadgeRepository _badgeRepository;
        private readonly IUserManager _userManager;
        private readonly IUserVotedBadgesRepository _userVotedBadgesRepository;
        private readonly UpdateSpecialBadgeCommandHandler _handler;

        public UpdateSpecialBadgeCommandHandlerTests()
        {
            _badgeRepository = Substitute.For<IBadgeRepository>();
            _userManager = Substitute.For<IUserManager>();
            _userVotedBadgesRepository = Substitute.For<IUserVotedBadgesRepository>();
            _handler = new UpdateSpecialBadgeCommandHandler(_badgeRepository, _userManager, _userVotedBadgesRepository);
        }

        [Fact]
        public async Task Handle_ValidationFailure_ReturnsErrorResponse()
        {
            // Arrange
            var command = new UpdateSpecialBadgeCommand { };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Success.Should().BeFalse();
            response.ValidationsErrors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var command = new UpdateSpecialBadgeCommand
            {
                VoterId = Guid.NewGuid(),
                VotedId = Guid.NewGuid(),
                Type = "Innovator"
            };
            _userManager.FindByIdAsync(Arg.Any<Guid>()).Returns(Task.FromResult(Result<UserDto>.Failure("Voter not found")));

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Success.Should().BeFalse();
            response.ValidationsErrors.Should().Contain("Voter not found"); // Updated to expect "Voter not found"
        }

        [Fact]
        public async Task Handle_Success_ReturnsSuccessResponse()
        {
            // Arrange
            var command = SetupValidCommand();
            SetupMocksForSuccess();

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Success.Should().BeTrue();
        }

        private UpdateSpecialBadgeCommand SetupValidCommand()
        {
            return new UpdateSpecialBadgeCommand
            {
                VoterId = Guid.NewGuid(),
                VotedId = Guid.NewGuid(),
                Type = "Innovator"
            };
        }

        private void SetupMocksForSuccess()
        {
            _userManager.FindByIdAsync(Arg.Any<Guid>()).Returns(Task.FromResult(Result<UserDto>.Success(new UserDto())));
            _badgeRepository.GetBadgeByUserIdAndType(Arg.Any<Guid>(), Arg.Any<string>())
                .Returns(Task.FromResult(Result<Badge>.Success(new Badge())));
            _userVotedBadgesRepository.GetUserVotedBadge(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<string>())
                .Returns(Task.FromResult(false));
            _userVotedBadgesRepository.AddAsync(Arg.Any<UserVotedBadges>())
                .Returns(Task.FromResult(Result<UserVotedBadges>.Success(new UserVotedBadges())));
            _badgeRepository.UpdateAsync(Arg.Any<Badge>())
                .Returns(Task.FromResult(Result<Badge>.Success(new Badge())));
        }
    }
}
