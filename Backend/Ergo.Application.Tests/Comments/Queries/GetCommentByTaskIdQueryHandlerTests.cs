using Ergo.Application.Features.Comments.Queries.GetByTaskId;
using Ergo.Application.Features.Users.Queries;
using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using NSubstitute;
namespace Ergo.Application.Tests.Comments.Queries
{
    public class GetCommentByTaskIdQueryHandlerTests
    {
        private readonly ICommentRepository _commentRepository = Substitute.For<ICommentRepository>();
        private readonly IUserManager _userManager = Substitute.For<IUserManager>();
        private readonly IUserPhotoRepository _userPhotoRepository = Substitute.For<IUserPhotoRepository>();
        private readonly GetCommentByTaskIdQueryHandler _handler;

        public GetCommentByTaskIdQueryHandlerTests()
        {
            _handler = new GetCommentByTaskIdQueryHandler(_commentRepository, _userManager, _userPhotoRepository);
        }

        [Fact]
        public async Task Handle_ReturnsComments_Successfully()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var commentCreationResult1 = Comment.Create("user1", taskId, "Test comment 1");
            var commentCreationResult2 = Comment.Create("user2", taskId, "Test comment 2");
            var comments = new List<Comment>
            {
                commentCreationResult1.Value,
                commentCreationResult2.Value
            };

            var successResult = Task.FromResult(Result<IReadOnlyList<Comment>>.Success(comments.AsReadOnly()));
            _commentRepository.GetCommentByTaskIdAsync(Arg.Any<Guid>()).Returns(successResult);

            foreach (var comment in comments)
            {
                _userManager.FindByUsernameAsync(comment.CreatedBy).Returns(Task.FromResult(Result<UserDto>.Success(new UserDto
                {
                    UserId = Guid.NewGuid().ToString(),
                    Username = comment.CreatedBy,
                    Name = "Test User",
                    Email = $"{comment.CreatedBy}@example.com"
                })));

                _userPhotoRepository.GetUserPhotoByUserIdAsync(Arg.Any<string>())
                    .Returns(Task.FromResult(Result<UserPhoto>.Success(UserPhoto.Create("http://example.com/photo.jpg", "userId").Value)));

            }

            var query = new GetCommentByTaskIdQuery { TaskId = taskId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Comments);
            Assert.Equal(comments.Count, result.Comments.Count);
        }
    }
}
