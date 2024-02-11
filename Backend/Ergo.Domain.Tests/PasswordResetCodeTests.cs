using Ergo.Domain.Entities;
using FluentAssertions;

namespace Ergo.Domain.Tests
{
    public class PasswordResetCodeTests
    {
        [Fact]
        public void DefaultConstructorTest()
        {
            // Arrange & Act
            var passwordResetCodeInstance = new PasswordResetCode();

            // Assert
            passwordResetCodeInstance.Should().NotBeNull();
        }
        [Fact]
        public void When_CreatePasswordResetCodeIsCalled_And_EmailIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = PasswordResetCode.Create("Test", "Test");
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreatePasswordResetCodeIsCalled_And_EmailIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = PasswordResetCode.Create(null, "Test");
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_CreatePasswordResetCodeIsCalled_And_CodeIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = PasswordResetCode.Create("Test", "Test");
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreatePasswordResetCodeIsCalled_And_CodeIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = PasswordResetCode.Create("Test", null);
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_IsValidIsCalled_And_ExpirationTimeIsGreaterThanCurrentTime_Then_TrueIsReturned()
        {
            //Arrange
            var passwordResetCode = PasswordResetCode.Create("Test", "Test");
            //Act
            var result = passwordResetCode.Value.IsValid();
            //Assert
            result.Should().BeTrue();
        }
        [Fact]
        public void When_IsValidIsCalled_And_ExpirationTimeIsLessThanCurrentTime_Then_FalseIsReturned()
        {
            //Arrange
            var passwordResetCode = PasswordResetCode.Create("Test", "Test");
            passwordResetCode.Value.Invalidate();
            //Act
            var result = passwordResetCode.Value.IsValid();
            //Assert
            result.Should().BeFalse();
        }

    }
}
