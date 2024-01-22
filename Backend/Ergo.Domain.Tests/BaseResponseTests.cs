using Ergo.Application.Responses;
using FluentAssertions;

namespace Ergo.Domain.Tests
{
    public class BaseResponseTests
    {
        [Fact]
        public void When_CreateBaseResponseIsCalled_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = new BaseResponse();
            //Assert
            result.Success.Should().BeTrue();
        }
        [Fact]
        public void When_CreateBaseResponseIsCalled_Then_MessageIsNull()
        {
            //Arrange && Act
            var result = new BaseResponse();
            //Assert
            result.Message.Should().BeNull();
        }
        [Fact]
        public void When_CreateBaseResponseIsCalled_Then_ValidationsErrorsIsNull()
        {
            //Arrange && Act
            var result = new BaseResponse();
            //Assert
            result.ValidationsErrors.Should().BeNull();
        }
        [Fact]
        public void When_CreateBaseResponseIsCalled_Then_MessageIsNotNull()
        {
            //Arrange && Act
            var result = new BaseResponse("Test", true);
            //Assert
            result.Message.Should().NotBeNull();
        }
       
        

    }
}
