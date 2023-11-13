using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ergo.Application.Persistence;

namespace Ergo.Application.Features.TaskItems.Queries.GetAll
{
    public class GetAllTaskItemsQueryHandler
    {
        private readonly ITaskItemRepository taskItemRepository;
        public GetAllTaskItemsQueryHandler(ITaskItemRepository taskItemRepository)
        {
            this.taskItemRepository = taskItemRepository;
        }

        public async Task<GetAllTaskItemsResponse> Handle(GetAllTaskItemsQuery request,
            CancellationToken cancellationToken)
        {
            GetAllTaskItemsResponse response = new();
            var result = await taskItemRepository.GetAllAsync();
            if (result.IsSuccess)
            {
                response.TaskItems = result.Value.Select(t => new TaskItemDto
                {
                    TaskName = t.TaskName,
                    Description = t.Description,
                    Deadline = t.Deadline,
                    FullName = t.CreatedBy,
                    ProjectId = t.ProjectId
                   
                }).ToList();
            }
            return response;
        }

    }
}
