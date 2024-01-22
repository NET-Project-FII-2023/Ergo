using Ergo.Domain.Common;

namespace Ergo.Domain.Tests
{
    public class ResultTests
    {
        [Fact]
        public void Result_Failure_SetsErrorProperty()
        {
            //Arrange && Act
            string errorMessage = "An error occurred";
            var result = Result<object>.Failure(errorMessage);
            //Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal(errorMessage, result.Error);
        }

    }
}
