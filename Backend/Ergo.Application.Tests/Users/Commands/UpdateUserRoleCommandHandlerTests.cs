using Ergo.Application.Features.Users.Commands.UpdateRole;
using Ergo.Application.Features.Users.Queries;
using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Tests.Users.Commands
{
    public class UpdateUserRoleCommandHandlerTests
    {
        private readonly IUserManager _userRepository;
        private readonly UpdateUserRoleCommandHandler _handler;
        public UpdateUserRoleCommandHandlerTests()
        {
            _userRepository = Substitute.For<IUserManager>();
            _handler = new UpdateUserRoleCommandHandler(_userRepository);
        }
        [Fact]
        public async Task Handle_UserExists_UpdatesSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new UpdateUserRoleCommand { UserId = userId, Role = "Admin" };
            var existingUser = Result<UserDto>.Success(new UserDto { UserId = userId.ToString() });
            _userRepository.FindByIdAsync(userId).Returns(Task.FromResult(existingUser));
            _userRepository.UpdateRoleAsync(Arg.Any<UserDto>(), Arg.Any<string>()).Returns(Task.FromResult(Result<UserDto>.Success(new UserDto())));
            // Act
            var response = await _handler.Handle(request, new CancellationToken());
            // Assert
            response.Success.Should().BeTrue();
        }
        [Fact]
        public async Task Handle_UpdateFails_ReturnsErrorResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new UpdateUserRoleCommand { UserId = userId, Role = "Admin" };
            var existingUser = Result<UserDto>.Success(new UserDto { UserId = userId.ToString() });
            _userRepository.FindByIdAsync(userId).Returns(Task.FromResult(existingUser));
            _userRepository.UpdateRoleAsync(Arg.Any<UserDto>(), Arg.Any<string>()).Returns(Task.FromResult(Result<UserDto>.Failure("Error")));
            // Act
            var response = await _handler.Handle(request, new CancellationToken());
            // Assert
            response.Success.Should().BeFalse();
        }
    }
}
