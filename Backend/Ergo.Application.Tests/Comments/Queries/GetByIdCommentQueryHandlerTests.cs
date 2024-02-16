using Ergo.Application.Features.Comments.Queries.GetById;
using Ergo.Application.Features.Users.Queries;
using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using NSubstitute;
namespace Ergo.Application.Tests.Comments.Queries
{
    public class GetByIdCommentQueryHandlerTests
    {
        private readonly ICommentRepository _commentRepository = Substitute.For<ICommentRepository>();
        private readonly IUserManager _userManager = Substitute.For<IUserManager>();
        private readonly IUserPhotoRepository _userPhotoRepository = Substitute.For<IUserPhotoRepository>();
        private readonly GetByIdCommentQueryHandler _handler;

        public GetByIdCommentQueryHandlerTests()
        {
            _handler = new GetByIdCommentQueryHandler(_commentRepository, _userManager, _userPhotoRepository);
        }

        [Fact]
        public async Task Handle_CommentAndUserFound_ReturnsSuccess()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var username = "testUser";
            var userId = "user123";
            var photoUrl = "http://example.com/photo.jpg";
            var commentResult = Comment.Create(username, taskId, "Sample comment text");
            var userDto = Result<UserDto>.Success(new UserDto { UserId = userId, Username = username });
            var userPhotoResult = UserPhoto.Create(photoUrl, userId);

            _commentRepository.FindByIdAsync(commentId).Returns(Task.FromResult(commentResult));
            _userManager.FindByUsernameAsync(username).Returns(Task.FromResult(userDto));
            _userPhotoRepository.GetUserPhotoByUserIdAsync(userId).Returns(Task.FromResult(userPhotoResult));

            var query = new GetByIdCommentQuery { CommentId = commentId };

            // Act
            var result = await _handler.Handle(query, default);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Comment);
            Assert.Equal(username, result.Comment.CreatedBy.Username);
            Assert.Equal(photoUrl, result.Comment.CreatedBy.UserPhoto.PhotoUrl);
        }

        [Fact]
        public async Task Handle_CommentNotFound_ReturnsError()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var commentError = "Comment not found";
            _commentRepository.FindByIdAsync(commentId).Returns(Task.FromResult(Result<Comment>.Failure(commentError)));

            var query = new GetByIdCommentQuery { CommentId = commentId };

            // Act
            var result = await _handler.Handle(query, default);

            // Assert
            Assert.False(result.Success);
            Assert.Contains(commentError, result.ValidationsErrors);
        }
    }
}
