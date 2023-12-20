using Ergo.Application.Features.Projects.Queries.GetProjectsByUserId;
using Ergo.Application.Persistence;
using Ergo.Application.Tests.RepositoryMocks;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Ergo.Application.Tests.Queries
{
    public class GetProjectsByUserIdQueryHandlerTests : IDisposable
    {
        private readonly IProjectRepository _mockProjectRepository;
        private readonly IUserRepository _mockUserRepository;
        private readonly GetProjectsByUserIdQueryHandler _handler;

        public GetProjectsByUserIdQueryHandlerTests()
        {
            _mockProjectRepository = ProjectRepositoryMocks.GetProjectRepository();
            _mockUserRepository = UserRepositoryMocks.GetUserRepository();
            _handler = new GetProjectsByUserIdQueryHandler(_mockProjectRepository, _mockUserRepository);
        }

        [Fact]
        public async Task Handle_ReturnsError_WhenUserDoesNotExist()
        {
            // Arrange
            var nonExistingUserId = Guid.NewGuid();
            var query = new GetProjectsByUserIdQuery { UserId = nonExistingUserId.ToString() };

            _mockUserRepository.FindByIdAsync(nonExistingUserId)
                .Returns(Task.FromResult(Result<User>.Failure("Not found")));

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Success.Should().BeFalse();
            response.ValidationsErrors.Should().ContainSingle().Which.Should().Be("Not found");
        }

        public void Dispose()
        {

        }
    }
}
