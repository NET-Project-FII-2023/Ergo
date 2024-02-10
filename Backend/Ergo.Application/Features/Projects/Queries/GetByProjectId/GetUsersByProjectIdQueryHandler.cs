using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.Users.Queries.GetByProjectId
{
    internal class GetUsersByProjectIdQueryHandler : IRequestHandler<GetUsersByProjectIdQuery, GetUsersByProjectIdQueryResponse>
    {
        private readonly IProjectRepository projectRepository;
        private readonly IUserManager userManager;
        private readonly IUserPhotoRepository userPhotoRepository;

        public GetUsersByProjectIdQueryHandler(IProjectRepository projectRepository, IUserManager userManager, IUserPhotoRepository userPhotoRepository)
        {
            this.projectRepository = projectRepository;
            this.userManager = userManager;
            this.userPhotoRepository = userPhotoRepository;
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

                var userPhoto = await userPhotoRepository.GetUserPhotoByUserIdAsync(user.UserId.ToString());
                usersDto.Add(new UserDto
                {
                    UserId = userById.Value.UserId,
                    Name = userById.Value.Name,
                    Username = userById.Value.Username,
                    Email = userById.Value.Email,
                    UserPhoto = userPhoto.IsSuccess ? new UserCloudPhotoDto
                    {
                        UserPhotoId = userPhoto.Value.UserPhotoId,
                        PhotoUrl = userPhoto.Value.PhotoUrl
                    } : null,
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