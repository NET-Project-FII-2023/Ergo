using Ergo.Application.Models.Statistics;
using Ergo.Application.Persistence;
using Ergo.Domain.Entities;
using MediatR;

namespace Ergo.Application.Features.Statistics.Queries.GetTasksDueThisWeek;

public class GetTasksDueThisWeekQueryHandler(
    IProjectRepository projectRepository,
    ITaskItemRepository taskItemRepository)
    : IRequestHandler<GetTasksDueThisWeekQuery, GetTasksDueThisWeekQueryResponse>
{
    public async Task<GetTasksDueThisWeekQueryResponse> Handle(GetTasksDueThisWeekQuery request, CancellationToken cancellationToken)
    {
        var projectsResult = await projectRepository.GetProjectsByUserId(request.UserId);
        
        if (!projectsResult.IsSuccess)
        {
            return new GetTasksDueThisWeekQueryResponse
            {
                Success = false,
                TasksDueThisWeek = [],
                Message = "Error getting projects by user id."
            };
        }
        
        var projects = projectsResult.Value;
        List<TaskDueModel> tasksDue = [];
        
        foreach (var project in projects)
        {
            var tasksResult = await taskItemRepository.GetTasksByProjectIdAsync(project.ProjectId);
            
            if (!tasksResult.IsSuccess)
            {
                return new GetTasksDueThisWeekQueryResponse
                {
                    Success = false,
                    TasksDueThisWeek = [],
                    Message = "Error getting tasks by project id."
                };
            }

            var tasks = tasksResult.Value;
            foreach (TaskItem task in tasks)
            {
                if (task.Deadline.HasValue && task.Deadline.Value.Date <= DateTime.Now.AddDays(7).Date)
                {
                    tasksDue.Add(new TaskDueModel
                    {
                        TaskItemId = task.TaskItemId,
                        ProjectId = task.ProjectId,
                        Deadline = task.Deadline.Value, 
                        State = task.State
                    });
                }
            }


        }

        return new GetTasksDueThisWeekQueryResponse
        {
            Success = true,
            TasksDueThisWeek = tasksDue,
            Message = "All tasks due this week."
        };
    }
}



