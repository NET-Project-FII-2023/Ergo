using Ergo.Application.Features.TaskItems.Queries.GetById;
using Ergo.Application.Persistence;
using Ergo.Application.Tests.RepositoryMocks;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Ergo.Application.Tests.TaskItems.Queries;

public class GetByIdTaskItemQueryHandlerTests : IDisposable
{
    private readonly ITaskItemRepository _mockTaskItemRepository;
    private readonly IUserPhotoRepository _mockUserPhotoRepository;
    private readonly IUserManager _mockUserManager;
    private readonly GetByIdTaskItemQueryHandler _handler;

    public GetByIdTaskItemQueryHandlerTests()
    {
        _mockTaskItemRepository = TaskItemRepositoryMocks.GetTaskItemRepository();
        _mockUserPhotoRepository = Substitute.For<IUserPhotoRepository>();
        _mockUserManager = Substitute.For<IUserManager>();
        _handler = new GetByIdTaskItemQueryHandler(_mockTaskItemRepository, _mockUserManager, _mockUserPhotoRepository);
    }
    
    /*[Fact]
    public async Task GetByIdTaskItemQueryHandler_ReturnsTaskItem_WhenTaskItemExists()
    {
        // Arrange
        var existingTaskItemId = TaskItemRepositoryMocks.TaskItems[0].TaskItemId;
        var query = new GetByIdTaskItemQuery(existingTaskItemId);
        this construtor has 0 parameters so how tf ???
        
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TaskItem.Should().NotBeNull();
        result.TaskItem.TaskItemId.Should().Be(existingTaskItemId);
        result.TaskItem.TaskName.Should().NotBeNullOrWhiteSpace();
    }*/

    [Fact]
    public async Task GetByIdTaskItemQueryHandler_ReturnsNull_WhenTaskItemDoesNotExist()
    {
        // Arrange
        var taskItem = TaskItemRepositoryMocks.TaskItems[0];

        // Act
        // GetByIdTaskItemQuery has 0 parameters ???
        var result = await _handler.Handle(new GetByIdTaskItemQuery(), CancellationToken.None);

        // Assert
        result.TaskItem.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdTaskItemQueryHandler_ThrowsException_WhenRepositoryFails()
    {
        // Arrange
        _mockTaskItemRepository.FindByIdAsync(Arg.Any<Guid>())
            .Returns(Task.FromException<Result<TaskItem>>(new Exception("Repository failure")));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(new GetByIdTaskItemQuery(), CancellationToken.None));
    }

    public void Dispose()
    {
    }
}