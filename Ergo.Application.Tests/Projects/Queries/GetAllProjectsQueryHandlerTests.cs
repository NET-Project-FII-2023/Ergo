using Ergo.Application.Features.Projects.Queries.GetAll;
using Ergo.Application.Persistence;
using Ergo.Application.Tests.RepositoryMocks;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Ergo.Application.Tests.Projects.Queries
{
    public class GetAllProjectsQueryHandlerTests : IDisposable
    {
        private readonly IProjectRepository _mockProjectRepository;
        private readonly GetAllProjectsQueryHandler _handler;

        public GetAllProjectsQueryHandlerTests()
        {
            _mockProjectRepository = ProjectRepositoryMocks.GetProjectRepository();
            _handler = new GetAllProjectsQueryHandler(_mockProjectRepository);
        }

        [Fact]
        public async Task GetAllProjectsQueryHandler_Success()
        {
            // Arrange

            // Act
            var result = await _handler.Handle(new GetAllProjectsQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Projects.Should().NotBeEmpty();
            result.Projects.Count.Should().Be(2);

            result.Projects.Should().ContainSingle(project => project.ProjectName == "Project 1");
            result.Projects.Should().ContainSingle(project => project.ProjectName == "Project 2");
        }

        [Fact]
        public async Task GetAllProjectsQueryHandler_ReturnsEmptyList_WhenNoProjectsExist()
        {
            // Arrange
            _mockProjectRepository.GetAllAsync().Returns(Result<IReadOnlyList<Project>>.Success(new List<Project>()));

            // Act
            var result = await _handler.Handle(new GetAllProjectsQuery(), CancellationToken.None);

            // Assert
            result.Projects.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllProjectsQueryHandler_ThrowsException_WhenRepositoryFails()
        {
            // Arrange
            _mockProjectRepository.GetAllAsync().Returns(Task.FromException<Result<IReadOnlyList<Project>>>(new Exception("Repository failure")));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(new GetAllProjectsQuery(), CancellationToken.None));
        }

        public void Dispose()
        {
        }
    }
}
