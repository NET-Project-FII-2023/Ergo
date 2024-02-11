using Ergo.Domain.Entities.Enums;
using Ergo.Domain.Entities;
using FluentAssertions;
using System.Reflection;

namespace Ergo.Domain.Tests
{
    public class TaskItemsTests
    {
        [Fact]
        public void When_CreateTaskItemIsCalled_And_TaskNameIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateTaskItemIsCalled_And_TaskNameIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create(null, "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_CreateTaskItemIsCalled_And_DescriptionIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateTaskItemIsCalled_And_DescriptionIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", null, DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_CreateTaskItemIsCalled_And_DeadlineIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateTaskItemIsCalled_And_DeadlineIsDefault_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", default, "Test", Guid.NewGuid(), null);
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_CreateTaskItemIsCalled_And_CreatedByIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateTaskItemIsCalled_And_CreatedByIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, null, Guid.NewGuid(), null);
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_CreateTaskItemIsCalled_And_ProjectIdIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateTaskItemIsCalled_And_ProjectIdIsEmpty_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.Empty, null);
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateTaskItemIsCalled_And_TaskNameIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            var updateResult = result.Value.UpdateData("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), TaskState.InProgress, null);
            //Assert
            updateResult.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void When_UpdateTaskItemIsCalled_And_TaskNameIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            var updateResult = result.Value.UpdateData(null, "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), TaskState.Done, null);
            //Assert
            updateResult.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void When_UpdateTaskItemIsCalled_And_DescriptionIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            var updateResult = result.Value.UpdateData("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), TaskState.InProgress, null);
            //Assert
            updateResult.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateTaskItemIsCalled_And_DescriptionIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            var updateResult = result.Value.UpdateData("Test", null, DateTime.UtcNow, "Test", Guid.NewGuid(), TaskState.Done, null);
            //Assert
            updateResult.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateTaskItemIsCalled_And_DeadlineIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            var updateResult = result.Value.UpdateData("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), TaskState.InProgress, null);
            //Assert
            updateResult.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateTaskItemIsCalled_And_DeadlineIsDefault_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            var updateResult = result.Value.UpdateData("Test", "Test", default, "Test", Guid.NewGuid(), TaskState.Done, null);
            //Assert
            updateResult.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateTaskItemIsCalled_And_CreatedByIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            var updateResult = result.Value.UpdateData("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), TaskState.InProgress, null);
            //Assert
            updateResult.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateTaskItemIsCalled_And_CreatedByIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            var updateResult = result.Value.UpdateData("Test", "Test", DateTime.UtcNow, null, Guid.NewGuid(), TaskState.Done, null);
            //Assert
            updateResult.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateTaskItemIsCalled_And_ProjectIdIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            var updateResult = result.Value.UpdateData("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), TaskState.InProgress, null);
            //Assert
            updateResult.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateTaskItemIsCalled_And_ProjectIdIsEmpty_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            var updateResult = result.Value.UpdateData("Test", "Test", DateTime.UtcNow, "Test", Guid.Empty, TaskState.Done, null);
            //Assert
            updateResult.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateTaskItemIsCalled_And_TaskStateIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            var updateResult = result.Value.UpdateData("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), TaskState.Done, null);
            //Assert
            updateResult.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateTaskItemIsCalled_And_TaskStateIsDefault_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            var updateResult =
                result.Value.UpdateData("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), default, null);
            //Assert
            updateResult.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_AssignCommentIsCalled_And_UserIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            var commentResult = Comment.Create("Test", Guid.NewGuid(), "Test");
            var assignResult = result.Value.AssignComment(commentResult.Value);
            //Assert
            assignResult.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_AssignCommentIsCalled_And_UserIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            var assignResult = result.Value.AssignComment(null);
            //Assert
            assignResult.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_AssignCommentToTaskItemIsCalled_And_CommentsIsNull_Then_CommentsIsInitializedAndCommentIsAdded()
        {
            // Arrange
            var taskItemResult = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            taskItemResult.Value.Comments = null; 

            var comment =  Comment.Create("Test Comment", Guid.NewGuid(),"Test"); 

            // Act
            var assignedResult = taskItemResult.Value.AssignComment(comment.Value);

            // Assert
            assignedResult.IsSuccess.Should().BeTrue();
            taskItemResult.Value.Comments.Should().NotBeNull();
            taskItemResult.Value.Comments.Should().Contain(comment.Value);
        }

        [Fact]
        public void When_AssignUserIsCalled_And_UserIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            var userResult = User.Create(Guid.NewGuid());
            var assignResult = result.Value.AssignUser(userResult.Value);
            //Assert
            assignResult.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_AssignUserIsCalled_And_UserIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            var assignResult = result.Value.AssignUser(null);
            //Assert
            assignResult.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void PrivateConstructorTest()
        {
            var constructor = typeof(TaskItem).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[0], null);
            var instance = constructor.Invoke(null);
            Assert.NotNull(instance);
        }
        [Fact]
        public void When_DeleteAssignedUserIsCalled_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            var userResult = User.Create(Guid.NewGuid());
            var assignResult = result.Value.AssignUser(userResult.Value);
            var deleteResult = result.Value.DeleteAssignedUser();
            //Assert
            deleteResult.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_StartOrResumeTaskItemIsCalled_And_TaskStateIsNotDone_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            var startResult = result.Value.StartOrResumeTask();
            //Assert
            startResult.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void When_StartOrResumeTaskIsCalled_And_TaskIsAlreadyInProgress_Then_FailureIsReturned()
        {
            // Arrange
            var task = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);

            task.Value.StartOrResumeTask(); // Starting the task once

            // Act
            var result = task.Value.StartOrResumeTask(); // Trying to start the task again

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Task is already in progress");
        }
        [Fact]
        public void When_PauseTaskIsCalled_And_TaskIsInProgress_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            result.Value.StartOrResumeTask();
            var pauseResult = result.Value.PauseTask();
            //Assert
            pauseResult.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_PauseTaskIsCalled_And_TaskIsNotInProgress_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            var pauseResult = result.Value.PauseTask();
            //Assert
            pauseResult.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_AddManualTimeIsCalled_And_TimeToAddIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            var addTimeResult = result.Value.AddManualTime(TimeSpan.FromMinutes(5));
            //Assert
            addTimeResult.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_AddManualTimeIsCalled_And_TimeToAddIsZero_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid(), null);
            var addTimeResult = result.Value.AddManualTime(TimeSpan.Zero);
            //Assert
            addTimeResult.IsSuccess.Should().BeFalse();
        }

    }
}
