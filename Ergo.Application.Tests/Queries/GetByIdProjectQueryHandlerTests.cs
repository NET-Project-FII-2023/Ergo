using Ergo.Application.Features.Projects.Queries.GetById;
using Ergo.Application.Persistence;
using Ergo.Application.Tests.RepositoryMocks;
using Ergo.Domain.Entities.Enums;
using FluentAssertions;
using Moq;
namespace Ergo.Application.Tests.Queries
{
    public class GetByIdProjectQueryHandlerTests : IDisposable
    {
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly GetByIdProjectQueryHandler _handler;

        public GetByIdProjectQueryHandlerTests()
        {
            _mockProjectRepository = ProjectRepositoryMocks.GetProjectRepository();
            _handler = new GetByIdProjectQueryHandler(_mockProjectRepository.Object);
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
            // Add more assertions to validate the properties are correctly mapped
        }

        [Fact]
        public async Task GetByIdProjectQueryHandler_ReturnsEmptyProject_WhenProjectDoesNotExist()
        {
            // Arrange
            var nonExistingProjectId = Guid.NewGuid();
            var query = new GetByIdProjectQuery(nonExistingProjectId);

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
            _mockProjectRepository.Setup(repo => repo.FindByIdAsync(projectId)).ThrowsAsync(new Exception("Repository failure"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));
        }


        public void Dispose()
        {
            _mockProjectRepository.Reset();
        }
    }
}
