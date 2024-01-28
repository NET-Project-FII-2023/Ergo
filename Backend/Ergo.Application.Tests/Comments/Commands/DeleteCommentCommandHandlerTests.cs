using NSubstitute;
using FluentAssertions;
using Ergo.Application.Features.Comments.Commands.DeleteComment;
using Ergo.Application.Persistence;
using System;
using System.Threading.Tasks;
using System.Threading;
using Xunit;
using Ergo.Domain.Entities;
using Ergo.Domain.Common;

public class DeleteCommentCommandHandlerTests
{
    private readonly ICommentRepository _commentRepository;
    private readonly DeleteCommentCommandHandler _handler;

    public DeleteCommentCommandHandlerTests()
    {
        _commentRepository = Substitute.For<ICommentRepository>();
        _handler = new DeleteCommentCommandHandler(_commentRepository);
    }

    [Fact]
    public async Task Handle_ValidComment_DeletesSuccessfully()
    {
        // Arrange
        var commentId = Guid.NewGuid();
        var comment = new Comment(); 
        _commentRepository.FindByIdAsync(commentId).Returns(Task.FromResult(Result<Comment>.Success(comment)));
        _commentRepository.DeleteAsync(commentId).Returns(Task.FromResult(Result<Comment>.Success(comment)));

        var request = new DeleteCommentCommand { CommentId = commentId };

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Success.Should().BeTrue();
    }



    [Fact]
    public async Task Handle_ValidationError_ReturnsErrors()
    {
        // Arrange
        var request = new DeleteCommentCommand { CommentId = Guid.Empty }; // Presupunem că acest ID este invalid

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Success.Should().BeFalse();
        response.ValidationsErrors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_DeleteFailure_ReturnsError()
    {
        // Arrange
        var commentId = Guid.NewGuid();
        var mockComment = Substitute.For<Comment>();
        _commentRepository.FindByIdAsync(commentId).Returns(Task.FromResult(Result<Comment>.Success(mockComment)));
        _commentRepository.DeleteAsync(commentId).Returns(Task.FromResult(Result<Comment>.Failure("Delete failed")));

        var request = new DeleteCommentCommand { CommentId = commentId };

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Success.Should().BeFalse();
        response.ValidationsErrors.Should().Contain("Delete failed");
    }

}
