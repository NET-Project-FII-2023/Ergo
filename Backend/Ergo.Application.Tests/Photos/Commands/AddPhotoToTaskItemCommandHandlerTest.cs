using Ergo.Application.Features.Photos.Commands.AddPhotoToTaskItem;
using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Tests.Photos.Commands
{
    public class AddPhotoToTaskItemCommandHandlerTest 
    {
        private readonly IPhotoRepository _mockPhotoRepository;
        private readonly ITaskItemRepository _mockTaskItemRepository;

        private readonly AddPhotoToTaskItemCommandHandler _handler;
        public AddPhotoToTaskItemCommandHandlerTest()
        {
            _mockPhotoRepository = Substitute.For<IPhotoRepository>();
            _mockTaskItemRepository = Substitute.For<ITaskItemRepository>();        
            _handler = new AddPhotoToTaskItemCommandHandler(_mockTaskItemRepository, _mockPhotoRepository);
        }

        [Fact]
        public async Task Handle_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var request = new AddPhotoToTaskItemCommand
            {
                TaskItemId = Guid.NewGuid(),
                CloudURL = "https://www.google.com",
            };

            _mockTaskItemRepository.FindByIdAsync(request.TaskItemId).Returns(Task.FromResult(Result<TaskItem>.Success(TaskItem.Create("TaskItem 1", "Description 1", DateTime.Now.AddDays(1), "FullName 1", Guid.NewGuid(), null).Value)));

            // Act
            var response = await _handler.Handle(request, new CancellationToken());

            // Assert
            response.Success.Should().BeTrue();
        }
        [Fact]
        public async Task Handle_WithInvalidTaskItem_ReturnsError()
        {
            // Arrange
            var request = new AddPhotoToTaskItemCommand
            {
                TaskItemId = Guid.NewGuid(),
                CloudURL = "https://www.google.com",
            };

            _mockTaskItemRepository.FindByIdAsync(request.TaskItemId).Returns(Task.FromResult(Result<TaskItem>.Failure("Not found")));

            // Act
            var response = await _handler.Handle(request, new CancellationToken());

            // Assert
            response.Success.Should().BeFalse();
        }
    }
}
