using Ergo.Domain.Entities;
using FluentAssertions;

namespace Ergo.Domain.Tests
{
    public class PhotoTests
    {
        [Fact]
        public void DefaultConstructorTest()
        {
            // Arrange & Act
            var photoInstance = new Photo();

            // Assert
            photoInstance.Should().NotBeNull();
        }
        [Fact]
        public void When_CreatePhotoIsCalled_And_UserIdIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = Photo.Create(Guid.NewGuid(), "Test");
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreatePhotoIsCalled_And_UserIdIsEmpty_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = Photo.Create(Guid.Empty, "Test");
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_CreatePhotoIsCalled_And_PhotoUrlIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = Photo.Create(Guid.NewGuid(), "Test");
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreatePhotoIsCalled_And_PhotoUrlIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = Photo.Create(Guid.NewGuid(), null);
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
    }
}
