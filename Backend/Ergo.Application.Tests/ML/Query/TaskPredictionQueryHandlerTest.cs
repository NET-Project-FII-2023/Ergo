using Ergo.Application.Features.ML.Query.GetTaskPrediction;
using Ergo.Application.Persistence;
using Ergo.Domain.Entities.ML;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Tests.ML.Query
{
    public class TaskPredictionQueryHandlerTest
    {
        private readonly IMLService _mockMLService;
        private readonly TaskPredictionQueryHandler _handler;
        public TaskPredictionQueryHandlerTest()
        {
            _mockMLService = Substitute.For<IMLService>();
            _handler = new TaskPredictionQueryHandler(_mockMLService);
        }

        [Fact]
        public async Task Handle_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var request = new TaskPredictionQuery
            {
                TaskData = new TaskDataDto { NumberOfParticipants = 1, TaskComplexity = 1, TaskUrgency = 1 }
            };

            _mockMLService.PredictCompletionTime(Arg.Any<TaskData>()).Returns(Task.FromResult(1.0f));

            // Act
            var response = await _handler.Handle(request, new CancellationToken());

            // Assert
            response.Success.Should().BeTrue();
            response.PredictedResolutionTime.Should().Be(1.0f);
        }
        [Fact]
        public async Task Handle_WithInvalidData_ReturnsError()
        {
            // Arrange
            var request = new TaskPredictionQuery
            {
                TaskData = new TaskDataDto { NumberOfParticipants = 0, TaskComplexity = 0, TaskUrgency = 0 }
            };

            _mockMLService.PredictCompletionTime(Arg.Any<TaskData>()).Returns(Task.FromResult(0.0f));

            // Act
            var response = await _handler.Handle(request, new CancellationToken());

            // Assert
            response.Success.Should().BeFalse();
        }
    }
}
