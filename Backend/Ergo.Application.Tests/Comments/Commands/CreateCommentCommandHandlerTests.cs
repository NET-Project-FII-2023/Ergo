using NSubstitute;
using FluentAssertions;
using Ergo.Application.Features.Comments.Commands.CreateComment;
using Ergo.Application.Persistence;
using System;
using System.Threading.Tasks;
using System.Threading;
using Xunit;
using FluentValidation.Results;
using Ergo.Domain.Entities;
using Ergo.Domain.Common;

public class CreateCommentCommandHandlerTests
{
    private readonly ICommentRepository _commentRepository;
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly CreateCommentCommandHandler _handler;
    
    public CreateCommentCommandHandlerTests()
    {
        _commentRepository = Substitute.For<ICommentRepository>();
        _taskItemRepository = Substitute.For<ITaskItemRepository>();
        _handler = new CreateCommentCommandHandler(_commentRepository, _taskItemRepository);
    }

    [Fact]
    public async Task Handle_WithValidData_ReturnsSuccess()
    {
        // Arrange
        var request = new CreateCommentCommand
        {
            CreatedBy = "John Doe",
            CreatedDate = DateTime.UtcNow,
            LastModifiedBy = "Jane Doe",
            LastModifiedDate = DateTime.UtcNow,
            CommentText = "This is a sample comment",
            TaskId = Guid.NewGuid() // Presupunând că TaskId este un Guid
        };

        _taskItemRepository.TaskItemExists(request.TaskId).Returns(Task.FromResult(true));

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Success.Should().BeTrue();
        response.Comment.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WithInvalidTaskItem_ReturnsError()
    {
        // Arrange
        var request = new CreateCommentCommand
        {
            CreatedBy = "John Doe",
            CreatedDate = DateTime.UtcNow,
            LastModifiedBy = "Jane Doe",
            LastModifiedDate = DateTime.UtcNow,
            CommentText = "This is a sample comment",
            TaskId = Guid.NewGuid()
        };

        _taskItemRepository.TaskItemExists(request.TaskId).Returns(Task.FromResult(false));

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Success.Should().BeFalse();
        response.ValidationsErrors.Should().Contain("TaskItem with the provided ID does not exist.");
    }



    [Fact]
    public async Task Handle_InvalidData_ReturnsValidationErrors()
    {
        // Arrange
        var request = new CreateCommentCommand
        {
            CreatedBy = "",
            CommentText = "" // Date invalide
        };

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Success.Should().BeFalse();
        response.ValidationsErrors.Should().NotBeEmpty();
    }


    [Fact]
    public async Task Handle_ValidData_CreatesComment()
    {
        // Arrange
        var request = new CreateCommentCommand
        {
            CreatedBy = "John Doe",
            CreatedDate = DateTime.UtcNow,
            LastModifiedBy = "Jane Doe",
            LastModifiedDate = DateTime.UtcNow,
            CommentText = "This is a sample comment",
            TaskId = Guid.NewGuid()
        };

        _taskItemRepository.TaskItemExists(request.TaskId).Returns(Task.FromResult(true));
        var successfulResult = Result<Comment>.Success(new Comment());
        _commentRepository.AddAsync(Arg.Any<Comment>()).Returns(Task.FromResult(successfulResult));


        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Success.Should().BeTrue();
        await _commentRepository.Received(1).AddAsync(Arg.Is<Comment>(c => c.CommentText == request.CommentText));
    }



}
