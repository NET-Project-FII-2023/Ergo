using Ergo.Application.Features.InboxItems.Commands.CreateInboxItem;
using Ergo.Application.Features.Users.Queries;
using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Tests.InboxItems.Commands
{
    public class CreateInboxItemHandlerTest
    {
        private readonly IInboxItemRepository _mockInboxItemRepository;
        private readonly IUserRepository _mockUserRepository;
        private readonly IUserManager _mockUserManager;

        private readonly CreateInboxItemCommandHandler _handler;
        public CreateInboxItemHandlerTest()
        {
            _mockInboxItemRepository = Substitute.For<IInboxItemRepository>();
            _mockUserRepository = Substitute.For<IUserRepository>();
            _mockUserManager = Substitute.For<IUserManager>();
            var fakeUserId = Guid.Parse("d1906196-01f7-4335-88b9-89f9672bb4ce");
            var fakeUserDto = new UserDto { UserId = fakeUserId.ToString(), Username = "John Doe" };
            _mockUserManager.FindByUsernameAsync("John Doe")
                .Returns(Task.FromResult(Result<UserDto>.Success(fakeUserDto)));
            var fakeUser = User.Create(fakeUserId).Value;
            _mockUserRepository.FindByIdAsync(fakeUserId)
                .Returns(Task.FromResult(Result<User>.Success(fakeUser)));
            _mockInboxItemRepository.AddAsync(Arg.Any<InboxItem>())
                .Returns(callInfo => Task.FromResult(Result<InboxItem>.Success((InboxItem)callInfo[0])));

            _handler = new CreateInboxItemCommandHandler(_mockInboxItemRepository, _mockUserRepository);

        }
        [Fact]
        public async Task Handle_WithValidData_ReturnsSuccess()
        {
            //Arrange
            var command = new CreateInboxItemCommand
            {
                UserId = Guid.Parse("d1906196-01f7-4335-88b9-89f9672bb4ce"),
                Message = "Test Message"
            };
            //Act
            var result = await _handler.Handle(command, new System.Threading.CancellationToken());
            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }
        [Fact]
        public async Task Handle_WithInvalidData_ReturnsFailure()
        {
            //Arrange
            var command = new CreateInboxItemCommand
            {
                UserId = Guid.NewGuid(),
                Message = ""
            };
            //Act
            var result = await _handler.Handle(command, new System.Threading.CancellationToken());
            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
        }
    }
}
