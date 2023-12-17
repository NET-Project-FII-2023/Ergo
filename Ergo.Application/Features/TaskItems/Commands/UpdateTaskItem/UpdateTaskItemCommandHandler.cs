using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ergo.Application.Persistence;
using MediatR;
using Ergo.Domain.Entities;

namespace Ergo.Application.Features.TaskItems.Commands.UpdateTaskItem
{
    public class UpdateTaskItemCommandHandler : IRequestHandler<UpdateTaskItemCommand, UpdateTaskItemCommandResponse>
    {
        private readonly ITaskItemRepository taskItemRepository;

        public UpdateTaskItemCommandHandler(ITaskItemRepository taskItemRepository)
        {
            this.taskItemRepository = taskItemRepository;
        }


        public async Task<UpdateTaskItemCommandResponse> Handle(UpdateTaskItemCommand request,
            CancellationToken cancellationToken)
        {
            Console.WriteLine(request.TaskItemId);
            var taskItem = await taskItemRepository.FindByIdAsync(request.TaskItemId);
            if (!taskItem.IsSuccess)
            {
                return new UpdateTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { taskItem.Error }
                };
            }   

            request.TaskName ??= taskItem.Value.TaskName;
            request.Description ??= taskItem.Value.Description;
            if (request.Deadline == default(DateTime))
            {
                request.Deadline = taskItem.Value.Deadline;
            }
            request.CreatedBy ??= taskItem.Value.CreatedBy;
            if (request.ProjectId == default(Guid))
            {
                request.ProjectId = taskItem.Value.ProjectId;
            }
            if (request.State == default)
            {
                request.State = taskItem.Value.State;
            }

            var validator = new UpdateTaskItemCommandValidator();
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validatorResult.IsValid)
            {
                return new UpdateTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            var updateResult = taskItem.Value.UpdateData(request.TaskName, request.Description, request.Deadline, request.CreatedBy, request.ProjectId, request.State);

            if (!updateResult.IsSuccess)
            {
                return new UpdateTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { updateResult.Error }
                };
            }

            var result = await taskItemRepository.UpdateAsync(taskItem.Value);
            return new UpdateTaskItemCommandResponse
            {
                Success = true,
                TaskItem = new UpdateTaskItemDto
                {
                    TaskName = result.Value.TaskName,
                    Description = result.Value.Description,
                    Deadline = result.Value.Deadline,
                    CreatedBy = result.Value.CreatedBy,
                    ProjectId = result.Value.ProjectId,
                    State = result.Value.State
                }
            };
        }
    }
}
