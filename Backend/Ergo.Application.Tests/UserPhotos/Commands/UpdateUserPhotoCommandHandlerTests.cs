using Ergo.Application.Features.UserPhotos.Commands.UpdateTaskPhoto;
using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Ergo.Application.Tests.UserPhotos.Commands
{
    public class UpdateUserPhotoCommandHandlerTests
    {
        private readonly IUserPhotoRepository _mockUserPhotoRepository;
        private readonly UpdateUserPhotoCommandHandler _handler;
        public UpdateUserPhotoCommandHandlerTests()
        {
            _mockUserPhotoRepository = Substitute.For<IUserPhotoRepository>();  
            _handler = new UpdateUserPhotoCommandHandler(_mockUserPhotoRepository);
        }
        [Fact]
        public async Task Handle_SuccessfulUpdate()
        {
            // Arrange
            var command = new UpdateUserPhotoCommand
            {
                UserPhotoId = Guid.NewGuid().ToString(),
                PhotoUrl = "https://example.com/photo.jpg"
            };
            var userPhoto = UserPhoto.Create("https://example.com/photo.jpg", Guid.NewGuid().ToString());
            _mockUserPhotoRepository.FindByIdAsync(Guid.Parse(command.UserPhotoId)).Returns(Result<UserPhoto>.Success(userPhoto.Value));
            // Act
            var response = await _handler.Handle(command, new CancellationToken());
            // Assert
            response.Success.Should().BeTrue();
        }
        [Fact]
        public async Task Handle_InvalidPhotoUrl_ReturnsErrorResponse()
        {
            // Arrange
            var command = new UpdateUserPhotoCommand
            {
                UserPhotoId = Guid.NewGuid().ToString(),
                PhotoUrl = " "
            };
            var userPhoto = UserPhoto.Create("https://example.com/photo.jpg", Guid.NewGuid().ToString());
            _mockUserPhotoRepository.FindByIdAsync(Guid.Parse(command.UserPhotoId)).Returns(Result<UserPhoto>.Success(userPhoto.Value));
            // Act
            var response = await _handler.Handle(command, new CancellationToken());
            // Assert
            response.Success.Should().BeFalse();
        }
        [Fact]
        public async Task Handle_InvalidUserPhotoId_ReturnsErrorResponse()
        {
            // Arrange
            var command = new UpdateUserPhotoCommand
            {
                UserPhotoId = Guid.NewGuid().ToString(),
                PhotoUrl = "https://example.com/photo.jpg"
            };
            _mockUserPhotoRepository.FindByIdAsync(Guid.Parse(command.UserPhotoId)).Returns(Result<UserPhoto>.Failure("Not found"));
            // Act
            var response = await _handler.Handle(command, new CancellationToken());
            // Assert
            response.Success.Should().BeFalse();
        }
    }
}
