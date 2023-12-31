﻿using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.TaskItems.Queries.GetById
{
    public class GetByIdTaskItemQueryHandler : IRequestHandler<GetByIdTaskItemQuery, GetByIdTaskItemQueryResponse>
    {
        private readonly ITaskItemRepository taskItemRepository;

        public GetByIdTaskItemQueryHandler(ITaskItemRepository taskItemRepository)
        {
            this.taskItemRepository = taskItemRepository;
        }

        public async Task<GetByIdTaskItemQueryResponse> Handle(GetByIdTaskItemQuery request, CancellationToken cancellationToken)
        {
            GetByIdTaskItemQueryResponse response = new();
            var taskItem = await taskItemRepository.FindByIdAsync(request.TaskItemId);

            if (!taskItem.IsSuccess)
            {
                response.Success = false;
                response.ValidationsErrors = new List<string> { taskItem.Error };
                return response;
            }

            return new GetByIdTaskItemQueryResponse
            {
                Success = true,
                TaskItem = new TaskItemDto
                {
                    TaskItemId = taskItem.Value.TaskItemId,
                    ProjectId = taskItem.Value.ProjectId,
                    BranchId = taskItem.Value.BranchId,
                    TaskName = taskItem.Value.TaskName,
                    Description = taskItem.Value.Description,
                    Deadline = taskItem.Value.Deadline,
                    State = taskItem.Value.State

                }
            };
        }
    }
}
