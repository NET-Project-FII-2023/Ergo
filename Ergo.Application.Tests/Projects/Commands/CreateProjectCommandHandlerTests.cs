using Ergo.Application.Features.Projects.Commands.CreateProject;
using Ergo.Application.Features.Users.Queries;
using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
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

            var fakeUserId = Guid.Parse("d1906196-01f7-4335-88b9-89f9672bb4ce");
            var fakeUserDto = new UserDto { UserId = fakeUserId.ToString(), Username = "John Doe" };

            // UserManager returns a successful result with a UserDto
            _mockUserManager.FindByUsernameAsync("John Doe")
                .Returns(Task.FromResult(Result<UserDto>.Success(fakeUserDto)));

            // UserRepository returns a successful result with a User
            var fakeUser = User.Create(fakeUserId).Value; // Use the static Create method to generate a valid User
            _mockUserRepository.FindByIdAsync(fakeUserId)
                .Returns(Task.FromResult(Result<User>.Success(fakeUser)));

            // Setup ProjectRepository's AddAsync to return a Result<Project> wrapped in a Task
            _mockProjectRepository.AddAsync(Arg.Any<Project>())
                .Returns(callInfo => Task.FromResult(Result<Project>.Success((Project)callInfo[0])));

            // UpdateAsync should also return a Result<Project> wrapped in a Task
            _mockProjectRepository.UpdateAsync(Arg.Any<Project>())
                .Returns(callInfo => Task.FromResult(Result<Project>.Success((Project)callInfo[0])));

            // Create the handler with the mocked dependencies
            _handler = new CreateProjectCommandHandler(_mockProjectRepository, _mockUserManager, _mockUserRepository);
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
