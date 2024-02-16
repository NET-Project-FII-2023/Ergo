using Ergo.Application.Features.Comments.Queries.GetAll;
using Ergo.Application.Features.Users.Queries;
using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using FluentAssertions;
using NSubstitute;
namespace Ergo.Application.Tests.Comments.Queries
{
    public class GetAllCommentsQueryHandlerTests
    {
        private readonly ICommentRepository _mockCommentRepository;
        private readonly IUserManager _mockUserManager;
        private readonly IUserPhotoRepository _mockUserPhotoRepository;
        private readonly GetAllCommentsQueryHandler _handler;

        public GetAllCommentsQueryHandlerTests()
        {
            _mockCommentRepository = Substitute.For<ICommentRepository>();
            _mockUserManager = Substitute.For<IUserManager>();
            _mockUserPhotoRepository = Substitute.For<IUserPhotoRepository>();
            _handler = new GetAllCommentsQueryHandler(_mockCommentRepository, _mockUserManager, _mockUserPhotoRepository);
        }

        [Fact]
        public async Task Handle_NoCommentsFound_ReturnsEmptyList()
        {
            // Arrange
            _mockCommentRepository.GetAllAsync().Returns(Task.FromResult(Result<IReadOnlyList<Comment>>.Success(new List<Comment>().AsReadOnly())));

            // Act
            var result = await _handler.Handle(new GetAllCommentsQuery(), CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            result.Comments.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_CommentsFoundButUserNotFound_SkipsComment()
        {
            // Arrange
            var comments = new List<Comment>
            {
                Comment.Create("username", Guid.NewGuid(), "Sample comment text").Value
            };
            _mockCommentRepository.GetAllAsync().Returns(Task.FromResult(Result<IReadOnlyList<Comment>>.Success(new List<Comment>().AsReadOnly())));
            _mockUserManager.FindByUsernameAsync(Arg.Any<string>()).Returns(Result<UserDto>.Failure("User not found"));

            // Act
            var result = await _handler.Handle(new GetAllCommentsQuery(), CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            result.Comments.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_CommentsAndUserPhotosFound_ReturnsCommentsWithPhotos()
        {
            // Arrange
            var comments = new List<Comment>
            {
                Comment.Create("username", Guid.NewGuid(), "Sample comment text").Value
            };
            var userDto = new UserDto { UserId = "userId", Username = "username", Name = "User Name", Email = "user@example.com" };
            var userPhoto = UserPhoto.Create("photoUrl", "userId").Value;

            _mockCommentRepository.GetAllAsync().Returns(Task.FromResult(Result<IReadOnlyList<Comment>>.Success(comments.AsReadOnly())));
            _mockUserManager.FindByUsernameAsync("username").Returns(Result<UserDto>.Success(userDto));
            _mockUserPhotoRepository.GetUserPhotoByUserIdAsync("userId").Returns(Result<UserPhoto>.Success(userPhoto));

            // Act
            var result = await _handler.Handle(new GetAllCommentsQuery(), CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            result.Comments.Should().NotBeEmpty();
            result.Comments.First().CreatedBy.UserPhoto.PhotoUrl.Should().Be("photoUrl");
        }

    }
}
