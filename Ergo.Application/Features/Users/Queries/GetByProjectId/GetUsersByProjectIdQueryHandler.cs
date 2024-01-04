using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.Users.Queries.GetByProjectId
{
    internal class GetUsersByProjectIdQueryHandler : IRequestHandler<GetUsersByProjectIdQuery, GetUsersByProjectIdQueryResponse>
    {
        private readonly IProjectRepository projectRepository;
        private readonly IUserManager userManager;

        public GetUsersByProjectIdQueryHandler(IProjectRepository projectRepository, IUserManager userManager)
        {
            this.projectRepository = projectRepository;
            this.userManager = userManager;
        }
        public async Task<GetUsersByProjectIdQueryResponse> Handle(GetUsersByProjectIdQuery request, CancellationToken cancellationToken)
        {
            var project = await projectRepository.FindByIdAsync(Guid.Parse(request.ProjectId));
            if (!project.IsSuccess)
            {
                return new GetUsersByProjectIdQueryResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { project.Error }
                };
            }
            var users = await projectRepository.GetUsersByProjectId(Guid.Parse(request.ProjectId));
            if (!users.IsSuccess)
            {
                return new GetUsersByProjectIdQueryResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { users.Error }
                };
            }
            //iterate through users and get the user by id
            var usersDto = new List<UserDto>();
            foreach (var user in users.Value)
            {
                var userById = await userManager.FindByIdAsync(Guid.Parse(user.UserId.ToString()));
                if (!userById.IsSuccess)
                {
                    return new GetUsersByProjectIdQueryResponse
                    {
                        Success = false,
                        ValidationsErrors = new List<string> { userById.Error }
                    };
                }
                usersDto.Add(new UserDto
                {
                    UserId = userById.Value.UserId,
                    Name = userById.Value.Name,
                    Username = userById.Value.Username,
                    Email = userById.Value.Email,
                    Roles = userById.Value.Roles
                });
            }
            return new GetUsersByProjectIdQueryResponse
            {
                Success = true,
                Users = usersDto
            };   
        }
    }
}
