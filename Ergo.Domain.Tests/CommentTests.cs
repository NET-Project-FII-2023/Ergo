using Ergo.Domain.Entities;
using FluentAssertions;
using System.ComponentModel;

namespace Ergo.Domain.Tests
{
    public class CommentTests
    {
        [Fact]
        public void When_CreateCommentIsCalled_And_CommentCreatedByIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = Comment.Create("Test",Guid.NewGuid(),"Test");
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateCommentIsCalled_And_CommentCreatedByIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = Comment.Create(null,Guid.NewGuid(),"Test");
            //Assert
            result.IsSuccess.Should().BeFalse();
            
        }
        [Fact]
        public void When_CreateCommentIsCalled_And_CommentTaskIdIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = Comment.Create("Test",Guid.NewGuid(),"Test");
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateCommentIsCalled_And_CommentTaskIdIsEmpty_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = Comment.Create("Test",Guid.Empty,"Test");
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_CreateCommentIsCalled_And_CommentTextIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = Comment.Create("Test",Guid.NewGuid(),"Test");
            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_CreateCommentIsCalled_And_CommentTextIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = Comment.Create("Test",Guid.NewGuid(),null);
            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateCommentIsCalled_And_CommentCreatedByIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = Comment.Create("Test",Guid.NewGuid(),"Test");
            var updateResult = result.Value.UpdateData("Test",DateTime.UtcNow,"Test",DateTime.UtcNow,"Test",Guid.NewGuid());
            //Assert
            updateResult.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateCommentIsCalled_And_CommentCreatedByIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = Comment.Create("Test",Guid.NewGuid(),"Test");
            var updateResult = result.Value.UpdateData(null,DateTime.UtcNow,"Test",DateTime.UtcNow,"Test",Guid.NewGuid());
            //Assert
            updateResult.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateCommentIsCalled_And_CommentTaskIdIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = Comment.Create("Test",Guid.NewGuid(),"Test");
            var updateResult = result.Value.UpdateData("Test",DateTime.UtcNow,"Test",DateTime.UtcNow,"Test",Guid.NewGuid());
            //Assert
            updateResult.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateCommentIsCalled_And_CommentTaskIdIsEmpty_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = Comment.Create("Test",Guid.NewGuid(),"Test");
            var updateResult = result.Value.UpdateData("Test",DateTime.UtcNow,"Test",DateTime.UtcNow,"Test",Guid.Empty);
            //Assert
            updateResult.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public void When_UpdateCommentIsCalled_And_CommentTextIsValid_Then_SuccessIsReturned()
        {
            //Arrange && Act
            var result = Comment.Create("Test",Guid.NewGuid(),"Test");
            var updateResult = result.Value.UpdateData("Test",DateTime.UtcNow,"Test",DateTime.UtcNow,"Test",Guid.NewGuid());
            //Assert
            updateResult.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public void When_UpdateCommentIsCalled_And_CommentTextIsNull_Then_FailureIsReturned()
        {
            //Arrange && Act
            var result = Comment.Create("Test",Guid.NewGuid(),"Test");
            var updateResult = result.Value.UpdateData("Test",DateTime.UtcNow,"Test",DateTime.UtcNow,null,Guid.NewGuid());
            //Assert
            updateResult.IsSuccess.Should().BeFalse();
        }
    }
}