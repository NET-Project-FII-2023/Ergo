using Ergo.Domain.Entities.Enums;
using Ergo.Domain.Entities;
using FluentAssertions;
using System.Reflection;

namespace Ergo.Domain.Tests
{
    public class ProjectTests
    {
        [Fact]
        public void When_CreateProjectIsCalled_And_ProjectNameIsValid_Then_SuccessIsReturned()
        {
            //Assert && Act
            var result = Project.Create("Test", "Test", null, DateTime.UtcNow, "Test");
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateProjectIsCalled_And_ProjectNameIsNull_Then_FailureIsReturned()
        {
            //Assert && Act
            var result = Project.Create(null, "Test", null, DateTime.UtcNow, "Test");
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_CreateProjectIsCalled_And_ProjectDescriptionIsValid_Then_SuccessIsReturned()
        {
            //Assert && Act
            var result = Project.Create("Test", "Test", null, DateTime.UtcNow, "Test");
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateProjectIsCalled_And_ProjectDescriptionIsNull_Then_FailureIsReturned()
        {
            //Assert && Act
            var result = Project.Create("Test", null, null, DateTime.UtcNow, "Test");
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_CreateProjectIsCalled_And_ProjectDeadlineIsValid_Then_SuccessIsReturned()
        {
            //Assert && Act
            var result = Project.Create("Test", "Test", null, DateTime.UtcNow, "Test");
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateProjectIsCalled_And_ProjectDeadlineIsDefault_Then_FailureIsReturned()
        {
            //Assert && Act
            var result = Project.Create("Test", "Test", null, default, "Test");
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_CreateProjectIsCalled_And_ProjectCreatorFullNameIsValid_Then_SuccessIsReturned()
        {
            //Assert && Act
            var result = Project.Create("Test", "Test", null, DateTime.UtcNow, "Test");
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateProjectIsCalled_And_ProjectCreatorFullNameIsNull_Then_FailureIsReturned()
        {
            //Assert && Act
            var result = Project.Create("Test", "Test", null, DateTime.UtcNow, null);
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateProjectIsCalled_And_ProjectNameIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = Project.Create("Test", "Test", null, DateTime.UtcNow, "Test");
            var updateResult = result.Value.UpdateData("Test", "Test", null, DateTime.UtcNow, ProjectState.Production, "Test");
            //Assert
            updateResult.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateProjectIsCalled_And_ProjectNameIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = Project.Create("Test", "Test", null, DateTime.UtcNow, "Test");
            var updateResult = result.Value.UpdateData(null, "Test", null, DateTime.UtcNow, ProjectState.Production, "Test");
            //Assert
            updateResult.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateProjectIsCalled_And_ProjectDescriptionIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = Project.Create("Test", "Test", null, DateTime.UtcNow, "Test");
            var updateResult = result.Value.UpdateData("Test", "Test", null, DateTime.UtcNow, ProjectState.Production, "Test");
            //Assert
            updateResult.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateProjectIsCalled_And_ProjectDescriptionIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = Project.Create("Test", "Test", null, DateTime.UtcNow, "Test");
            var updateResult = result.Value.UpdateData("Test", null, null, DateTime.UtcNow, ProjectState.Production, "Test");
            //Assert
            updateResult.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateProjectIsCalled_And_ProjectDeadlineIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = Project.Create("Test", "Test", null, DateTime.UtcNow, "Test");
            var updateResult = result.Value.UpdateData("Test", "Test", null, DateTime.UtcNow, ProjectState.Production, "Test");
            //Assert
            updateResult.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateProjectIsCalled_And_ProjectDeadlineIsDefault_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = Project.Create("Test", "Test", null, DateTime.UtcNow, "Test");
            var updateResult = result.Value.UpdateData("Test", "Test", null, default, ProjectState.Production, "Test");
            //Assert
            updateResult.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateProjectIsCalled_And_ProjectCreatorFullNameIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = Project.Create("Test", "Test", null, DateTime.UtcNow, "Test");
            var updateResult = result.Value.UpdateData("Test", "Test", null, DateTime.UtcNow, ProjectState.Production, "Test");
            //Assert
            updateResult.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void When_UpdateProjectIsCalled_And_ProjectCreatorFullNameIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = Project.Create("Test", "Test", null, DateTime.UtcNow, "Test");
            var updateResult = result.Value.UpdateData("Test", "Test", null, DateTime.UtcNow, ProjectState.Production, null);
            //Assert
            updateResult.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateProjectIsCalled_And_ProjectStateIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = Project.Create("Test", "Test", null, DateTime.UtcNow, "Test");
            var updateResult = result.Value.UpdateData("Test", "Test", null, DateTime.UtcNow, ProjectState.Production, "Test");
            //Assert
            updateResult.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateProjectIsCalled_And_ProjectStateIsDefault_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = Project.Create("Test", "Test", null, DateTime.UtcNow, "Test");
            var updateResult = result.Value.UpdateData("Test", "Test", null, DateTime.UtcNow, default, "Test");
            //Assert
            updateResult.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void When_AssignUserToProjectIsCalled_And_UserIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = Project.Create("Test", "Test", "Test", DateTime.UtcNow, "Test");
            var userResult = User.Create(Guid.NewGuid());
            var assignedResult = result.Value.AssignUser(userResult.Value);
            //Assert
            assignedResult.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_AssignUserToProjectIsCalled_And_UserIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = Project.Create("Test", "Test", "Test", DateTime.UtcNow, "Test");
            var assignedResult = result.Value.AssignUser(null);
            //Assert
            assignedResult.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void PrivateConstructorTest()
        {
            //Arrange && Act
            var constructor = typeof(Project).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[0], null);
            var instance = constructor.Invoke(null);
            //Assert
            Assert.NotNull(instance);
        }
        [Fact]
        public void StartDateIsSetCorrectly()
        {
            //Arrange && Act
            var result = Project.Create("Test", "Test", null, DateTime.UtcNow, "Test");
            var project = result.Value;

            var startDateField = project.GetType().GetProperty("StartDate", BindingFlags.Instance | BindingFlags.Public);
            var startDateValue = (DateTime)startDateField.GetValue(project);
            //Assert
            startDateValue.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }


    }
}
