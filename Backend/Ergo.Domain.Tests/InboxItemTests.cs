using Ergo.Domain.Entities;
using FluentAssertions;

namespace Ergo.Domain.Tests
{
    public class InboxItemTests
    {
        [Fact]
        public void DefaultConstructorTest()
        {
            // Arrange & Act
            var inboxItemInstance = new InboxItem();

            // Assert
            inboxItemInstance.Should().NotBeNull();
        }
        [Fact]
        public void When_CreateInboxItemIsCalled_And_UserIdIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = InboxItem.Create(Guid.NewGuid(), "Test");
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateInboxItemIsCalled_And_UserIdIsEmpty_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = InboxItem.Create(Guid.Empty, "Test");
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_CreateInboxItemIsCalled_And_MessageIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = InboxItem.Create(Guid.NewGuid(), "Test");
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateInboxItemIsCalled_And_MessageIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = InboxItem.Create(Guid.NewGuid(), null);
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateIsReadIsCalled_And_IsReadIsValid_Then_SuccessIsReturned()
        {
            //Arrange
            var inboxItem = InboxItem.Create(Guid.NewGuid(), "Test").Value;
            //Act
            var result = inboxItem.UpdateIsRead(true);
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}
