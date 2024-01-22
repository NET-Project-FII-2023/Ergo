using Ergo.Application.Features.TaskItems.Commands.CreateTaskItem;
using Ergo.Application.Persistence;
using Ergo.Application.Tests.RepositoryMocks;
using Ergo.Domain.Entities.Enums;
using FluentAssertions;
using NSubstitute;

namespace Ergo.Application.Tests.TaskItems.Commands;

public class CreateTaskItemCommandHandlerTests
{
    private readonly ITaskItemRepository _mockTaskItemRepository;
    private readonly IProjectRepository _mockProjectRepository;
    private readonly CreateTaskItemCommandHandler _handler;

    public CreateTaskItemCommandHandlerTests()
    {
        _mockTaskItemRepository = TaskItemRepositoryMocks.GetTaskItemRepository();
        _mockProjectRepository = ProjectRepositoryMocks.GetProjectRepository();
        _handler = new CreateTaskItemCommandHandler(_mockTaskItemRepository, _mockProjectRepository);
    }

    [Fact]
    public async Task Handle_WithValidData_ReturnsSuccess()
    {
        // Arrange
        var request = new CreateTaskItemCommand
        {
            TaskName = "Task 1",
            CreatedBy = "Marcel",
            Description = "This is a sample description",
            Deadline = DateTime.UtcNow.AddDays(5),
            ProjectId = Guid.NewGuid(),
            State = TaskState.ToDo
        };

        _mockProjectRepository.ProjectExists(request.ProjectId).Returns(Task.FromResult(true));

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Success.Should().BeTrue();
        response.TaskItem.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WithInvalidProject_ReturnsError()
    {
        // Arrange
        var request = new CreateTaskItemCommand
        {
            TaskName = "Task 1",
            CreatedBy = "Marcel",
            Description = "This is a sample description",
            Deadline = DateTime.UtcNow.AddDays(5),
            ProjectId = Guid.NewGuid(),
            State = TaskState.ToDo
        };

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Success.Should().BeFalse();
        response.ValidationsErrors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_WithInvalidData_ReturnsError()
    {
        // Arrange
        var request = new CreateTaskItemCommand
        {
            TaskName = "Task 1",
            CreatedBy = "Marcel",
            Description = "This is a sample description",
            Deadline = DateTime.UtcNow.AddDays(5),
            ProjectId = Guid.NewGuid(),
            State = TaskState.ToDo
        };
        
        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Success.Should().BeFalse();
        response.ValidationsErrors.Should().NotBeEmpty();
    }
}