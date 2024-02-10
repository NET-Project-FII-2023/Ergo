using Ergo.Application.Features.Users.Queries;
using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.TaskItems.Queries.GetByUserId;

public class GetTasksByProjectsOfUsersQueryHandler : IRequestHandler<GetTasksByProjectsOfUsersQuery, GetTasksByProjectsOfUsersQueryResponse>
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IUserManager _userManager;
    
    public GetTasksByProjectsOfUsersQueryHandler(ITaskItemRepository taskItemRepository, IProjectRepository projectRepository, IUserManager userManager)
    {
        _taskItemRepository = taskItemRepository;
        _projectRepository = projectRepository;
        _userManager = userManager;
    }
    
    public async Task<GetTasksByProjectsOfUsersQueryResponse> Handle(GetTasksByProjectsOfUsersQuery request, CancellationToken cancellationToken)
    {
        //get all projects of user
        var projectsResult = await _projectRepository.GetProjectsByUserId(request.UserId);
        
        if (!projectsResult.IsSuccess)
        {
            return new GetTasksByProjectsOfUsersQueryResponse
            {
                Success = false,
                TaskItems = null
            };
        }
        
        //get all tasks of these projects and assign them to the project id key in the dictionary
        var projects = projectsResult.Value;
        var taskItems = new Dictionary<Guid, List<TaskItemDto>>();
        
        foreach (var project in projects)
        {
            var taskItemsResult = await _taskItemRepository.GetTasksByProjectIdAsync(project.ProjectId);
            
            if (!taskItemsResult.IsSuccess)
            {
                return new GetTasksByProjectsOfUsersQueryResponse
                {
                    Success = false,
                    TaskItems = null
                };
            }
            
            var tasks = taskItemsResult.Value;
            var taskItemDtos = new List<TaskItemDto>();
            
            foreach (var task in tasks)
            {
                UserTaskDto? assignedUser = null;
                var assignedUserId = await _taskItemRepository.GetAssignedUser(task.TaskItemId);

                if (assignedUserId.IsSuccess)
                {
                    var guidUserId = Guid.Empty;
                    if (!Guid.TryParse(assignedUserId.Value, out guidUserId))
                    {
                        continue;
                    }
                
                    var user = await _userManager.FindByIdAsync(guidUserId);
                    if (!user.IsSuccess)
                    {
                        continue;
                    }
                    
                    assignedUser = new UserTaskDto
                    {
                        UserId = user.Value.UserId,
                        Username = user.Value.Username,
                        Name = user.Value.Name
                    };
                }
                
                var taskItemDto = new TaskItemDto
                {
                    TaskItemId = task.TaskItemId,
                    BranchId = task.BranchId,
                    TaskName = task.TaskName,
                    Description = task.Description,
                    Deadline = task.Deadline,
                    CreatedBy = task.CreatedBy,
                    ProjectId = task.ProjectId,
                    AssignedUser = assignedUser,
                    State = task.State
                };
                
                taskItemDtos.Add(taskItemDto);
            }
            
            taskItems.Add(project.ProjectId, taskItemDtos);
        }
        
        return new GetTasksByProjectsOfUsersQueryResponse
        {
            Success = true,
            TaskItems = taskItems
        };
    }
}