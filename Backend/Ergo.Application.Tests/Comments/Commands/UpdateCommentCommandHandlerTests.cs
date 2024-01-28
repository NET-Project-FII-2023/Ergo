using NSubstitute;
using FluentAssertions;
using Ergo.Application.Features.Comments.Commands.UpdateComment;
using Ergo.Application.Persistence;
using System;
using System.Threading.Tasks;
using System.Threading;
using Xunit;
using FluentValidation.Results;
using Ergo.Domain.Entities;
using Ergo.Domain.Common;

public class UpdateCommentCommandHandlerTests
{
    private readonly ICommentRepository _commentRepository;
    private readonly UpdateCommentCommandHandler _handler;

    public UpdateCommentCommandHandlerTests()
    {
        _commentRepository = Substitute.For<ICommentRepository>();
        _handler = new UpdateCommentCommandHandler(_commentRepository);
    }


    [Fact]
    public async Task Handle_CommentDoesNotExist_ReturnsError()
    {
        // Arrange
        var request = new UpdateCommentCommand { CommentId = Guid.NewGuid() };
        _commentRepository.FindByIdAsync(request.CommentId).Returns(Task.FromResult(Result<Comment>.Failure("Comment not found")));

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Success.Should().BeFalse();
        response.ValidationsErrors.Should().Contain("Comment with id this does not exists");
    }

    [Fact]
    public async Task Handle_InvalidData_ReturnsValidationErrors()
    {
        // Arrange
        var commentId = Guid.NewGuid();
        var existingComment = new Comment();
        _commentRepository.FindByIdAsync(commentId).Returns(Task.FromResult(Result<Comment>.Success(existingComment)));

        var request = new UpdateCommentCommand
        {
            CommentId = commentId,
            // Lăsați unele câmpuri invalide sau necompletate
        };

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Success.Should().BeFalse();
        response.ValidationsErrors.Should().NotBeEmpty();
    }


}