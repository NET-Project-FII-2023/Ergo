using Ergo.Application.Persistence;
using Ergo.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ergo.Domain.Entities.Enums;
using Ergo.Application.Features.TaskItems.Queries.GetByProjectId;

namespace Ergo.Application.Features.TaskItems.Queries.GetTasksByProjectId
{
    public class GetTasksByProjectIdQueryHandler : IRequestHandler<GetTasksByProjectIdQuery, GetTasksByProjectIdQueryResponse>
    {
        private readonly ITaskItemRepository _taskItemRepository;

        public GetTasksByProjectIdQueryHandler(ITaskItemRepository taskItemRepository)
        {
            _taskItemRepository = taskItemRepository;
        }

        public async Task<GetTasksByProjectIdQueryResponse> Handle(GetTasksByProjectIdQuery request, CancellationToken cancellationToken)
        {
            var taskItemsResult = await _taskItemRepository.GetTasksByProjectIdAsync(request.ProjectId);

            if (!taskItemsResult.IsSuccess)
            {
                // Handle the case where fetching task items by project ID fails
                return new GetTasksByProjectIdQueryResponse
                {
                    Success = false,
                    // Handle error or return empty task items list
                    // TaskItems = new List<TaskItemDto>() or TaskItems = null
                };
            }

            var taskItems = taskItemsResult.Value;

            var taskItemDtos = new List<TaskItemDto>();

            foreach (var taskItem in taskItems)
            {
                taskItemDtos.Add(new TaskItemDto
                {
                    TaskItemId = taskItem.TaskItemId,
                    BranchId = taskItem.BranchId,
                    TaskName = taskItem.TaskName,
                    Description = taskItem.Description,
                    Deadline = taskItem.Deadline,
                    ProjectId = taskItem.ProjectId,
                    State = taskItem.State
                });
            }

            return new GetTasksByProjectIdQueryResponse
            {
                Success = true,
                TaskItems = taskItemDtos
            };
        }

    }
}
