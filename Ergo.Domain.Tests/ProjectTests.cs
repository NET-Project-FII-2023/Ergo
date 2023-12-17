using Ergo.Domain.Entities;
using Ergo.Domain.Entities.Enums;
using FluentAssertions;

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
            var result = Project.Create("Test", "Test", "Test", DateTime.UtcNow, "Test");
            var updateResult = result.Value.UpdateData("Test", "Test", "Test", DateTime.UtcNow, ProjectState.Production, "Test");
            //Assert
            updateResult.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateProjectIsCalled_And_ProjectStateIsDefault_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = Project.Create("Test", "Test", null, DateTime.UtcNow, "Test");
            var updateResult = result.Value.UpdateData("Test", "Test", null, DateTime.UtcNow, default, null);
            //Assert
            updateResult.IsSuccess.Should().BeFalse();
        }
    }
}
