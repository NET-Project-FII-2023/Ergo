using Ergo.Application.Features.InboxItems.Commands.UpdateInboxItemIsRead;
using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Ergo.Application.Tests.InboxItems.Commands
{
    public class UpdateInboxItemIsReadCommandHandlerTests
    {
        private readonly IInboxItemRepository _mockInboxItemRepository;
        private readonly UpdateInboxItemIsReadCommandHandler _handler;
        public UpdateInboxItemIsReadCommandHandlerTests()
        {
            _mockInboxItemRepository = Substitute.For<IInboxItemRepository>();
            _handler = new UpdateInboxItemIsReadCommandHandler(_mockInboxItemRepository);
        }
        [Fact]
        public async Task Handle_WithValidData_ReturnsSuccess()
        {
            //Arrange
            var command = new UpdateInboxItemIsReadCommand
            {
                InboxItemId = Guid.NewGuid()
            };
            var inboxItem = InboxItem.Create(Guid.NewGuid(), "Test Message").Value;
            _mockInboxItemRepository.FindByIdAsync(command.InboxItemId)
                .Returns(Task.FromResult(Result<InboxItem>.Success(inboxItem)));
            var inboxItemDto = new InboxItemDto
            {
                UserId = inboxItem.UserId,
                CreatedDate = inboxItem.CreatedDate,
                Message = inboxItem.Message,
                IsRead = true
            };  
            _mockInboxItemRepository.UpdateIsReadAsync(inboxItem, true)
                .Returns(Task.FromResult(Result<InboxItemDto>.Success(inboxItemDto)));
            
            


            //Act
            var result = await _handler.Handle(command, new CancellationToken());
            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }
        [Fact]
        public async Task Handle_WithInvalidData_ReturnsFailure()
        {
            //Arrange
            var command = new UpdateInboxItemIsReadCommand
            {
                InboxItemId = Guid.NewGuid()
            };
            var inboxItem = InboxItem.Create(Guid.NewGuid(), "Test Message").Value;
            _mockInboxItemRepository.FindByIdAsync(command.InboxItemId)
                .Returns(Task.FromResult(Result<InboxItem>.Failure("Not found")));
            //Act
            var result = await _handler.Handle(command, new CancellationToken());
            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
        }
    }
}
