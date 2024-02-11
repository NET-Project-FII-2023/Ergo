using Ergo.Domain.Entities;
using FluentAssertions;
using System.Reflection;

namespace Ergo.Domain.Tests
{
    public class BadgeTests
    {
        [Fact]
        public void PrivateConstructorTest()
        {
            // Arrange
            var paramTypes = new Type[] { typeof(string), typeof(int),typeof(Guid), typeof(string) };
            var constructor = typeof(Badge).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, paramTypes, null);
            var parameters = new object[] { "Test", 1, Guid.NewGuid(), "Innovator" };

            // Act
            var instance = constructor.Invoke(parameters);

            // Assert
            Assert.NotNull(instance);

        }
        [Fact]
        public void DefaultConstructorTest()
        {
            // Arrange & Act
            var badgeInstance = new Badge();

            // Assert
            badgeInstance.Should().NotBeNull();
        }
        [Fact]
        public void When_CreateBadgeIsCalled_And_NameIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = Badge.Create("Test", 1, Guid.NewGuid(), "Innovator");
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateBadgeIsCalled_And_NameIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = Badge.Create(null, 1, Guid.NewGuid(), "Innovator");
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_CreateBadgeIsCalled_And_CountIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = Badge.Create("Test", 1, Guid.NewGuid(), "Innovator");
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateBadgeIsCalled_And_CountIsNegative_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = Badge.Create("Test", -1, Guid.NewGuid(), "Innovator");
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_CreateBadgeIsCalled_And_UserIdIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = Badge.Create("Test", 1, Guid.NewGuid(), "Innovator");
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateBadgeIsCalled_And_UserIdIsEmpty_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = Badge.Create("Test", 1, Guid.Empty, "Innovator");
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_CreateBadgeIsCalled_And_TypeIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = Badge.Create("Test", 1, Guid.NewGuid(), "Innovator");
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateBadgeIsCalled_And_TypeIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = Badge.Create("Test", 1, Guid.NewGuid(), null);
            //Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void When_UpdateCountIsCalled_And_CountIsValid_Then_SuccessIsReturned()
        {
            //Arrange
            var badge = Badge.Create("Test", 1, Guid.NewGuid(), "Innovator").Value;
            //Act
            var result = badge.UpdateCount(2);
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateCountIsCalled_And_CountIsNegative_Then_FailureIsReturned()
        {
            //Arrange
            var badge = Badge.Create("Test", 1, Guid.NewGuid(), "Innovator").Value;
            //Act
            var result = badge.UpdateCount(-1);
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateCountIsCalled_And_CountIsValid_And_TypeIsInnovator_Then_ActiveIsTrue()
        {
            //Arrange
            var badge = Badge.Create("Test", 1, Guid.NewGuid(), "Innovator").Value;
            //Act
            badge.UpdateCount(2);
            //Assert
            badge.Active.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateCountIsCalled_And_CountIsValid_And_TypeIsNotInnovator_Then_ActiveIsFalse()
        {
            //Arrange
            var badge = Badge.Create("Test", 1, Guid.NewGuid(), "Test").Value;
            //Act
            badge.UpdateCount(2);
            //Assert
            badge.Active.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateCountIsCalled_And_CountIsValid_And_TypeIsQualityMaster_Then_ActiveIsTrue()
        {
            //Arrange
            var badge = Badge.Create("Test", 1, Guid.NewGuid(), "Quality-Master").Value;
            //Act
            badge.UpdateCount(2);
            //Assert
            badge.Active.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateCountIsCalled_And_CountIsValid_And_TypeIsNotQualityMaster_Then_ActiveIsFalse()
        {
            //Arrange
            var badge = Badge.Create("Test", 1, Guid.NewGuid(), "Test").Value;
            //Act
            badge.UpdateCount(2);
            //Assert
            badge.Active.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateCountIsCalled_And_CountIsValid_And_TypeIsProblemSolver_Then_ActiveIsTrue()
        {
            //Arrange
            var badge = Badge.Create("Test", 1, Guid.NewGuid(), "Problem-Solver").Value;
            //Act
            badge.UpdateCount(2);
            //Assert
            badge.Active.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateCountIsCalled_And_CountIsValid_And_TypeIsNotProblemSolver_Then_ActiveIsFalse()
        {
            //Arrange
            var badge = Badge.Create("Test", 1, Guid.NewGuid(), "Test").Value;
            //Act
            badge.UpdateCount(2);
            //Assert
            badge.Active.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateCountIsCalled_And_CountIsValid_And_TypeIsTeamPlayer_Then_ActiveIsTrue()
        {
            //Arrange
            var badge = Badge.Create("Test", 1, Guid.NewGuid(), "Team-Player").Value;
            //Act
            badge.UpdateCount(2);
            //Assert
            badge.Active.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateCountIsCalled_And_CountIsValid_And_TypeIsNotTeamPlayer_Then_ActiveIsFalse()
        {
            //Arrange
            var badge = Badge.Create("Test", 1, Guid.NewGuid(), "Test").Value;
            //Act
            badge.UpdateCount(2);
            //Assert
            badge.Active.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateCountIsCalled_And_CountIsValid_And_TypeIsTaskDone_And_CountIsBiggerThenOneHundred_Then_ActiveIsTrue()
        {
            //Arrange
            var badge = Badge.Create("Test", 1, Guid.NewGuid(), "TasksDone").Value;
            //Act
            badge.UpdateCount(101);
            //Assert
            badge.Active.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateCountIsCalled_And_CountIsValid_And_TypeIsTaskDone_And_CountIsLessThenOneHundred_Then_ActiveIsFalse()
        {
            //Arrange
            var badge = Badge.Create("Test", 1, Guid.NewGuid(), "TasksDone").Value;
            //Act
            badge.UpdateCount(99);
            //Assert
            badge.Active.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateCountIsCalled_And_CountIsValid_And_TypeIsNotTaskDone_Then_ActiveIsFalse()
        {
            //Arrange
            var badge = Badge.Create("Test", 1, Guid.NewGuid(), "Test").Value;
            //Act
            badge.UpdateCount(2);
            //Assert
            badge.Active.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateCountIsCalled_And_CountIsValid_And_TypeIsCommentsMade_AndCountIsBiggerThanFifty_Then_ActiveIsTrue()
        {
            //Arrange
            var badge = Badge.Create("Test", 1, Guid.NewGuid(), "CommentsMade").Value;
            //Act
            badge.UpdateCount(51);
            //Assert
            badge.Active.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateCountIsCalled_And_CountIsValid_And_TypeIsCommentsMade_AndCountIsLessThanFifty_Then_ActiveIsFalse()
        {
            //Arrange
            var badge = Badge.Create("Test", 1, Guid.NewGuid(), "CommentsMade").Value;
            //Act
            badge.UpdateCount(49);
            //Assert
            badge.Active.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateCountIsCalled_And_CountIsValid_And_TypeIsProjectsMade_AndCountIsBiggerThanTen_Then_ActiveIsTrue()
        {
            //Arrange
            var badge = Badge.Create("Test", 1, Guid.NewGuid(), "ProjectsMade").Value;
            //Act
            badge.UpdateCount(10);
            //Assert
            badge.Active.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateCountIsCalled_And_CountIsValid_And_TypeIsProjectsMade_AndCountIsLessThanTen_Then_ActiveIsFalse()
        {
            //Arrange
            var badge = Badge.Create("Test", 1, Guid.NewGuid(), "ProjectsMade").Value;
            //Act
            badge.UpdateCount(9);
            //Assert
            badge.Active.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateCountIsCalled_And_CountIsValid_And_TypeIsHoursWorked_AndCountIsBiggerThanOneHundred_Then_ActiveIsTrue()
        {
            //Arrange
            var badge = Badge.Create("Test", 1, Guid.NewGuid(), "HoursWorked").Value;
            //Act
            badge.UpdateCount(101);
            //Assert
            badge.Active.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateCountIsCalled_And_CountIsValid_And_TypeIsHoursWorked_AndCountIsLessThanOneHundred_Then_ActiveIsFalse()
        {
            //Arrange
            var badge = Badge.Create("Test", 1, Guid.NewGuid(), "HoursWorked").Value;
            //Act
            badge.UpdateCount(99);
            //Assert
            badge.Active.Should().BeFalse();
        }





    }
}
