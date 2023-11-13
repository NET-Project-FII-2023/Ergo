using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.TaskItems.Queries.GetById
{
    public class GetByIdTaskItemQueryHandler : IRequestHandler<GetByIdTaskItemQuery, TaskItemDto>
    {
        
        private readonly ITaskItemRepository _taskItemRepository;

        public GetByIdTaskItemQueryHandler(ITaskItemRepository taskItemRepository)
        {
            _taskItemRepository = taskItemRepository;
        }

        public async Task<TaskItemDto> Handle(GetByIdTaskItemQuery request, CancellationToken cancellationToken)
        {
            var taskItem = await _taskItemRepository.FindByIdAsync(request.Id);
            if (taskItem.IsSuccess)
            {
                return new TaskItemDto
                {
                    TaskName = taskItem.Value.TaskName,
                    Description = taskItem.Value.Description,
                    Deadline = taskItem.Value.Deadline, 
                    FullName = taskItem.Value.CreatedBy,
                    ProjectId = taskItem.Value.ProjectId
                };
            }
            return new TaskItemDto();
        }   
    }
}
