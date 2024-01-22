using Ergo.Application.Persistence;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ergo.Application.Features.TaskItems.Commands.DeleteTaskItem
{
    public class DeleteTaskItemCommandHandler : IRequestHandler<DeleteTaskItemCommand, DeleteTaskItemCommandResponse>
    {
        private readonly ITaskItemRepository repository;

        public DeleteTaskItemCommandHandler(ITaskItemRepository repository)
        {
            this.repository = repository;
        }

        public async Task<DeleteTaskItemCommandResponse> Handle(DeleteTaskItemCommand request, CancellationToken cancellationToken)
        {
            DeleteTaskItemCommandResponse response = new();
            var validator = new DeleteTaskItemCommandValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.ValidationsErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return response;
            }

            var taskItemToDelete = await repository.FindByIdAsync(request.TaskItemId);

            if (taskItemToDelete == null)
            {
                response.Success = false;
                response.ValidationsErrors = new List<string> { "TaskItem not found" };
                return response;
            }

            var result = await repository.DeleteAsync(request.TaskItemId);

            if (!result.IsSuccess)
            {
                response.Success = false;
                response.ValidationsErrors = new List<string> { result.Error };
                return response;
            }

            return new DeleteTaskItemCommandResponse
            {
                Success = true
            };
        }
    }
}
