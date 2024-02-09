using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using NSubstitute;

namespace Ergo.Application.Tests.RepositoryMocks;

public static class TaskItemRepositoryMocks
{
    internal readonly static List<TaskItem> TaskItems =
    [
        TaskItem.Create("TaskItem 1", "Description 1", DateTime.Now.AddDays(1), "FullName 1", Guid.NewGuid(), null).Value,
        TaskItem.Create("TaskItem 2", "Description 2", DateTime.Now.AddDays(2), "FullName 2", Guid.NewGuid(), null).Value,
    ];
    
    public static ITaskItemRepository GetTaskItemRepository()
    {
        var mockTaskItemRepository = Substitute.For<ITaskItemRepository>();

        mockTaskItemRepository.GetAllAsync().Returns(Result<IReadOnlyList<TaskItem>>.Success(TaskItems));
        mockTaskItemRepository.FindByIdAsync(TaskItems[0].TaskItemId)
            .Returns(Result<TaskItem>.Success(TaskItems[0]));
        mockTaskItemRepository.FindByIdAsync(TaskItems[1].TaskItemId)
            .Returns(Result<TaskItem>.Success(TaskItems[1]));

        mockTaskItemRepository.FindByIdAsync(Arg.Is<Guid>(id => id != TaskItems[0].TaskItemId && id != TaskItems[1].TaskItemId))
            .Returns(Result<TaskItem>.Failure("Not found"));

        return mockTaskItemRepository;
    }
}