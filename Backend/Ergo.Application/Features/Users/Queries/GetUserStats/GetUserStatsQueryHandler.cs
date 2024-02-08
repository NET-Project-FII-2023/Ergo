using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.Users.Queries.GetUserStats
{
    public class GetUserStatsQueryHandler : IRequestHandler<GetUserStatsQuery, GetUserStatsQueryResponse>
    {
        private readonly IUserManager userManager;
        private readonly IProjectRepository projectRepository;
        private readonly ITaskItemRepository taskRepository;
        private readonly ICommentRepository commentRepository;
        public GetUserStatsQueryHandler(IUserManager userManager, IProjectRepository projectRepository, ITaskItemRepository taskRepository, ICommentRepository commentRepository)
        {
            this.userManager = userManager;
            this.projectRepository = projectRepository;
            this.taskRepository = taskRepository;
            this.commentRepository = commentRepository;
        }

        public async Task<GetUserStatsQueryResponse> Handle(GetUserStatsQuery request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(Guid.Parse(request.UserId));
            if (!user.IsSuccess)
            {
                return new GetUserStatsQueryResponse
                {
                    Success = false,
                    Message = "User not found"
                };  
            }
            var numberOfProjects = await projectRepository.GetNumberOfProjectsByUserIdAsync(Guid.Parse(request.UserId));
            var numberOfTasks = await taskRepository.GetNumberOfTasksByUserIdAsync(Guid.Parse(request.UserId));
            var numberOfComments = await commentRepository.GetNumberOfCommentsByUserIdAsync(user.Value.Username);
            var totalHoursWorked = await taskRepository.GetTotalHoursWorkedByUserIdAsync(Guid.Parse(request.UserId));
            return new GetUserStatsQueryResponse
            {
                Success = true,
                UserStats = new UserStatsDto
                {
                    TotalProjectsCreated = numberOfProjects,
                    TotalTasks = numberOfTasks,
                    TotalComments = numberOfComments,
                    TotalHoursWorked = totalHoursWorked
                }
            };


        }
    }
}
