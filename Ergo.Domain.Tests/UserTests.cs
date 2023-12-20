using Ergo.Domain.Entities;
using Ergo.Domain.Entities.Enums;
using FluentAssertions;

namespace Ergo.Domain.Tests
{
    public class UserTests
    {
        [Fact]
        public void When_CreateUserIsCalled_And_UserIdIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = User.Create(Guid.NewGuid());
            //Assert
            result.IsSuccess.Should().BeTrue();  
        }
        [Fact]
        public void When_CreateUserIsCalled_And_UserIdIsEmpty_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = User.Create(Guid.Empty);
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_AssignProjectIsCalled_And_ProjectIsValid_Then_SuccessIsReturned()
        {
            //Arrange
            var user = User.Create(Guid.NewGuid());
            var project = Project.Create("Test", "Test", null, DateTime.UtcNow, "Test");
            //Act
            var result = user.Value.AssignProject(project.Value);
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_AssignProjectIsCalled_And_ProjectIsNull_Then_FailureIsReturned()
        {
            //Arrange
            var user = User.Create(Guid.NewGuid());
            //Act
            var result = user.Value.AssignProject(null);
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_AssignTaskIsCalled_And_TaskIsValid_Then_SuccessIsReturned()
        {
            //Arrange
            var user = User.Create(Guid.NewGuid());
            var task = TaskItem.Create("Test", "Test", DateTime.UtcNow, "Test", Guid.NewGuid());
            //Act
            var result = user.Value.AssignTask(task.Value);
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_AssignTaskIsCalled_And_TaskIsNull_Then_FailureIsReturned()
        {
            //Arrange
            var user = User.Create(Guid.NewGuid());
            //Act
            var result = user.Value.AssignTask(null);
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
    }
}
