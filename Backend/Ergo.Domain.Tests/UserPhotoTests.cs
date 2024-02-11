using Ergo.Domain.Entities;
using FluentAssertions;

namespace Ergo.Domain.Tests
{
    public class UserPhotoTests 
    {
        [Fact]
        public void DefaultConstructorTest()
        {
            // Arrange & Act
            var userPhotoInstance = new UserPhoto();

            // Assert
            userPhotoInstance.Should().NotBeNull();
        }
        [Fact]
        public void When_CreateUserPhotoIsCalled_And_PhotoUrlIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = UserPhoto.Create("Test", "Test");
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateUserPhotoIsCalled_And_PhotoUrlIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = UserPhoto.Create(null, "Test");
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_CreateUserPhotoIsCalled_And_UserIdIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = UserPhoto.Create("Test", "Test");
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateUserPhotoIsCalled_And_UserIdIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = UserPhoto.Create("Test", null);
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateUserPhotoIsCalled_And_PhotoUrlIsValid_Then_SuccessIsReturned()
        {
            //Arrange 
            var result = UserPhoto.Create("Test", "Test");
            //Act
            var update = result.Value.UpdatePhoto("Test");
            //Assert
            update.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateUserPhotoIsCalled_And_PhotoUrlIsNull_Then_FailureIsReturned()
        {
            //Arrange
            var result = UserPhoto.Create("Test", "Test");
            //Act   
            var update = result.Value.UpdatePhoto("");
            //Assert
            update.IsSuccess.Should().BeFalse();
        }
    }
}
