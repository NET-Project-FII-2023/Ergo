using Ergo.Application.Features.Projects.Queries.GetById;
using Ergo.Application.Persistence;
using Ergo.Application.Tests.RepositoryMocks;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Ergo.Application.Tests.Projects.Queries
{
    public class GetByIdProjectQueryHandlerTests : IDisposable
    {
        private readonly IProjectRepository _mockProjectRepository;
        private readonly GetByIdProjectQueryHandler _handler;

        public GetByIdProjectQueryHandlerTests()
        {
            _mockProjectRepository = ProjectRepositoryMocks.GetProjectRepository();
            _handler = new GetByIdProjectQueryHandler(_mockProjectRepository);
        }

        [Fact]
        public async Task GetByIdProjectQueryHandler_ReturnsProject_WhenProjectExists()
        {
            // Arrange
            var existingProjectId = ProjectRepositoryMocks.Projects[0].ProjectId;
            var query = new GetByIdProjectQuery(existingProjectId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.ProjectId.Should().Be(existingProjectId);
            result.ProjectName.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task GetByIdProjectQueryHandler_ReturnsEmptyProject_WhenProjectDoesNotExist()
        {
            // Arrange
            var nonExistingProjectId = Guid.NewGuid();
            var query = new GetByIdProjectQuery(nonExistingProjectId);

            _mockProjectRepository.FindByIdAsync(nonExistingProjectId)
                .Returns(Task.FromResult(Result<Project>.Failure("Not found")));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.ProjectId.Should().Be(Guid.Empty);
            result.ProjectName.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdProjectQueryHandler_ThrowsException_WhenRepositoryFails()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var query = new GetByIdProjectQuery(projectId);

            _mockProjectRepository.FindByIdAsync(projectId)
                .Returns(Task.FromException<Result<Project>>(new Exception("Repository failure")));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));
        }


        public void Dispose()
        {
        }
    }
}
