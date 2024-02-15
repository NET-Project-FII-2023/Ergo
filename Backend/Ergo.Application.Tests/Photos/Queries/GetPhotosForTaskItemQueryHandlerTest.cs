using Ergo.Application.Features.Photos.Queries.GetPhotosForTaskItem;
using Ergo.Application.Features.TaskItems.Queries;
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

namespace Ergo.Application.Tests.Photos.Queries
{
    public  class GetPhotosForTaskItemQueryHandlerTest
    {
        private readonly IPhotoRepository _mockPhotoRepository;
        private readonly ITaskItemRepository _mockTaskItemRepository;
        private readonly GetPhotosForTaskItemQueryHandler _handler;
        public GetPhotosForTaskItemQueryHandlerTest()
        {
            _mockPhotoRepository = Substitute.For<IPhotoRepository>();
            _mockTaskItemRepository = Substitute.For<ITaskItemRepository>();
            var fakeTaskId = Guid.Parse("d1906196-01f7-4335-88b9-89f9672bb4ce");
            var fakeTaskItemDto = new TaskItemDto { TaskItemId = fakeTaskId, ProjectId = Guid.NewGuid(), State = Domain.Entities.Enums.TaskState.ToDo };

            var fakeTaskItem = TaskItem.Create(fakeTaskItemDto.TaskName, fakeTaskItemDto.Description, DateTime.Now.AddDays(1), fakeTaskItemDto.CreatedBy, fakeTaskItemDto.ProjectId, "Test").Value;
            

            _handler = new GetPhotosForTaskItemQueryHandler(_mockPhotoRepository, _mockTaskItemRepository);
        }
        [Fact]
        public async Task Handle_WithValidData_ReturnsSuccess()
        {
            //Arrange & Act
            var result = await _handler.Handle(new GetPhotosForTaskItemQuery(), new System.Threading.CancellationToken());

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Photos.Count.Should().Be(2);

            result.Photos.Should().ContainSingle(photo => photo.CloudURL == "https://www.google.com");
            result.Photos.Should().ContainSingle(photo => photo.CloudURL == "https://www.google.com");

        }
    }
}
