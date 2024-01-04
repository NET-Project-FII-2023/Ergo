using Ergo.Application.Features.Projects.Commands.CreateProject;
using Ergo.Application.Persistence;
using FluentAssertions;
using NSubstitute;

namespace Ergo.Application.Tests.Projects.Commands
{
    public class CreateProjectCommandHandlerTests : IDisposable
    {
        private readonly IProjectRepository _mockProjectRepository;
        private readonly IUserManager _mockUserManager;
        private readonly IUserRepository _mockUserRepository;

        private readonly CreateProjectCommandHandler _handler;


        public CreateProjectCommandHandlerTests()
        {
            _mockProjectRepository = Substitute.For<IProjectRepository>();
            _mockUserManager = Substitute.For<IUserManager>();
            _mockUserRepository = Substitute.For<IUserRepository>();

            _handler = new CreateProjectCommandHandler(_mockProjectRepository,_mockUserManager,_mockUserRepository);
        }

        [Fact]
        public async Task Handle_SuccessfulCreation()
        {
            // Arrange
            var command = new CreateProjectCommand
            {
                ProjectName = "New Project", 
                Description = "Description",
                GitRepository = "https://example.com/repo.git",
                FullName = "John Doe",
                Deadline = DateTime.Now.AddDays(10)
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Project.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_ValidationFailure()
        {
            // Arrange
            var command = new CreateProjectCommand
            {
                ProjectName = "New Project",
                Description = "Description",
                GitRepository = "https://example.com/repo.git",
                FullName = "John Doe",
                Deadline = DateTime.Now.AddDays(-10)
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.ValidationsErrors.Should().NotBeEmpty();
            
        }

        public void Dispose()
        {
        }
    }
}
