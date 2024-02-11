using Ergo.Application.Features.TaskItems.Queries.GetAll;
using Ergo.Application.Persistence;
using Ergo.Application.Tests.RepositoryMocks;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Ergo.Application.Tests.TaskItems.Queries;

public class GetAllTaskItemsQueryHandlerTests
{
    private readonly ITaskItemRepository _mockTaskItemRepository;
    private readonly IUserManager _mockUserManager;
    private readonly GetAllTaskItemsQueryHandler _handler;

    public GetAllTaskItemsQueryHandlerTests()
    {
        _mockTaskItemRepository = TaskItemRepositoryMocks.GetTaskItemRepository();
        //_mockUserManager = UserManagerRepositoryMocks.GetUserManager();
        _handler = new GetAllTaskItemsQueryHandler(_mockTaskItemRepository,_mockUserManager);
    }

    [Fact]
    public async Task GetAllTaskItemsQueryHandler_Success()
    {
        // Arrange

        // Act
        var result = await _handler.Handle(new GetAllTaskItemsQuery(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TaskItems.Should().NotBeEmpty();
        result.TaskItems.Count.Should().Be(2);

        result.TaskItems.Should().ContainSingle(taskItem => taskItem.TaskName == "TaskItem 1");
        result.TaskItems.Should().ContainSingle(taskItem => taskItem.TaskName == "TaskItem 2");
    }

    [Fact]
    public async Task GetAllTaskItemsQueryHandler_ReturnsEmptyList_WhenNoTaskItemsExist()
    {
        // Arrange
        _mockTaskItemRepository.GetAllAsync().Returns(Result<IReadOnlyList<TaskItem>>.Success(new List<TaskItem>()));

        // Act
        var result = await _handler.Handle(new GetAllTaskItemsQuery(), CancellationToken.None);

        // Assert
        result.TaskItems.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllTaskItemsQueryHandler_ThrowsException_WhenRepositoryFails()
    {
        // Arrange
        _mockTaskItemRepository.GetAllAsync().Returns(Task.FromException<Result<IReadOnlyList<TaskItem>>>(new Exception("Repository failure")));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(new GetAllTaskItemsQuery(), CancellationToken.None));
    }

    public void Dispose()
    {
    }
    
}