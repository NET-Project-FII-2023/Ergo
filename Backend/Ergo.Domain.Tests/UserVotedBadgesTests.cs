using Ergo.Domain.Entities;
using FluentAssertions;

namespace Ergo.Domain.Tests
{
    public class UserVotedBadgesTests
    {
        [Fact]
        public void DefaultConstructorTest()
        {
            // Arrange & Act
            var userVotedBadgesInstance = new UserVotedBadges();

            // Assert
            userVotedBadgesInstance.Should().NotBeNull();
        }
        [Fact]
        public void When_CreateUserVotedBadgesIsCalled_And_UserIdIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = UserVotedBadges.Create(Guid.NewGuid(),Guid.NewGuid(), "Test");
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateUserVotedBadgesIsCalled_And_UserIdIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = UserVotedBadges.Create(Guid.Empty, Guid.NewGuid(), "Test");
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_CreateUserVotedBadgesIsCalled_And_VotedIdIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = UserVotedBadges.Create(Guid.NewGuid(), Guid.NewGuid(), "Test");
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateUserVotedBadgesIsCalled_And_VotedIdIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = UserVotedBadges.Create(Guid.NewGuid(), Guid.Empty, "Test");
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_CreateUserVotedBadgesIsCalled_And_TypeIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = UserVotedBadges.Create(Guid.NewGuid(), Guid.NewGuid(), "Test");
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateUserVotedBadgesIsCalled_And_TypeIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = UserVotedBadges.Create(Guid.NewGuid(), Guid.NewGuid(), null);
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        
    }
}
