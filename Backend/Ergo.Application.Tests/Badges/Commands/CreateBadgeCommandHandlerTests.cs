using Ergo.Application.Features.Badges.Commands.CreateBadgeCommand;
using Ergo.Application.Features.Users.Queries;
using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using FluentAssertions;
using NSubstitute;

namespace Ergo.Application.Tests.Badges.Commands
{
    public class CreateBadgeCommandHandlerTests
    {
        private readonly IBadgeRepository _badgeRepository;
        private readonly IUserManager _userManager;
        private readonly CreateBadgeCommandHandler _handler;

        public CreateBadgeCommandHandlerTests()
        {
            _badgeRepository = Substitute.For<IBadgeRepository>();
            _userManager = Substitute.For<IUserManager>();
            _handler = new CreateBadgeCommandHandler(_badgeRepository, _userManager);
        }

        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ReturnsError()
        {
            // Arrange
            var command = new CreateBadgeCommand
            {
                Name = "Test Badge",
                Count = 1,
                UserId = Guid.NewGuid(),
                Type = "Innovator"
            };

            _userManager.FindByIdAsync(command.UserId).Returns(Task.FromResult(Result<UserDto>.Failure("User not found")));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.ValidationsErrors.Should().ContainSingle().Which.Should().Be("User not found");
        }

        [Fact]
        public async Task Handle_WithInvalidData_ReturnsValidationErrors()
        {
            // Arrange
            var command = new CreateBadgeCommand { };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.ValidationsErrors.Should().NotBeEmpty();
        }

    }
}
