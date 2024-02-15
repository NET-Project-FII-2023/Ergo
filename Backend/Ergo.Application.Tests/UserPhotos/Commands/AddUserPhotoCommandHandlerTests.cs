using Ergo.Application.Features.UserPhotos.Commands.AddUserPhoto;
using Ergo.Application.Persistence;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Tests.UserPhotos.Commands
{
    public class AddUserPhotoCommandHandlerTests
    {
        private readonly IUserPhotoRepository _mockUserPhotoRepository;
        private readonly AddUserPhotoCommandHandler _handler;
        public AddUserPhotoCommandHandlerTests()
        {
            _mockUserPhotoRepository = Substitute.For<IUserPhotoRepository>();  
            _handler = new AddUserPhotoCommandHandler(_mockUserPhotoRepository);
        }
        [Fact]
        public async Task Handle_SuccessfulCreation()
        {
            // Arrange
            var command = new AddUserPhotoCommand
            {
                PhotoUrl = "https://example.com/photo.jpg",
                UserId = Guid.NewGuid().ToString()
            };
            // Act
            var response = await _handler.Handle(command, new CancellationToken());
            // Assert
            response.Success.Should().BeTrue();
        }
        [Fact]
        public async Task Handle_InvalidPhotoUrl_ReturnsErrorResponse()
        {
            // Arrange
            var command = new AddUserPhotoCommand
            {
                PhotoUrl = " ",
                UserId = Guid.NewGuid().ToString()
            };
            // Act
            var response = await _handler.Handle(command, new CancellationToken());
            // Assert
            response.Success.Should().BeFalse();
        }
        [Fact]
        public async Task Handle_InvalidUserId_ReturnsErrorResponse()
        {
            // Arrange
            var command = new AddUserPhotoCommand
            {
                PhotoUrl = "https://example.com/photo.jpg",
                UserId = " "
            };
            // Act
            var response = await _handler.Handle(command, new CancellationToken());
            // Assert
            response.Success.Should().BeFalse();
        }
    }
}
