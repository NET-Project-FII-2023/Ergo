using Ergo.Application.Features.Users.Commands.DeleteUser;
using Ergo.Application.Features.Users.Queries;
using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using FluentAssertions;
using NSubstitute;

namespace Ergo.Application.Tests.Users.Commands
{
    public  class DeleteUserCommandHandlerTests
    {
        private readonly IUserManager _mockUserManager;
        private readonly DeleteUserCommandHandler _handler;
        public DeleteUserCommandHandlerTests()
        {
            _mockUserManager = Substitute.For<IUserManager>();
            _handler = new DeleteUserCommandHandler(_mockUserManager);
        }
        [Fact]
        public async Task Handle_UserExists_DeletesSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new DeleteUserCommand { UserId = userId };
            _mockUserManager.DeleteAsync(userId).Returns(Result<UserDto>.Success(new UserDto()));
            // Act
            var response = await _handler.Handle(request, new CancellationToken());
            // Assert
            response.Success.Should().BeTrue();
        }
        [Fact]
        public async Task Handle_DeleteFails_ReturnsErrorResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new DeleteUserCommand { UserId = userId };
            _mockUserManager.DeleteAsync(userId).Returns(Result<UserDto>.Failure("Not found"));
            // Act
            var response = await _handler.Handle(request, new CancellationToken());
            // Assert
            response.Success.Should().BeFalse();
        }

    }
}
