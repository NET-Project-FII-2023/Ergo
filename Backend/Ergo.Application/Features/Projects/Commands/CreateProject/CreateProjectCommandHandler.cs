using Ergo.Application.Features.Projects.Commands.AssignUserToProject;
using Ergo.Application.Persistence;
using Ergo.Domain.Entities;
using MediatR;
namespace Ergo.Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, CreateProjectCommandResponse>
    {
        private readonly IProjectRepository projectRepository;
        private readonly IUserManager userManager;
        private readonly IUserRepository userRepository;

        public CreateProjectCommandHandler(IProjectRepository projectRepository, IUserManager userManager, IUserRepository userRepository)
        {
            this.projectRepository = projectRepository;
            this.userManager = userManager;
            this.userRepository = userRepository;
        }

        public async Task<CreateProjectCommandResponse> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateProjectCommandValidator();
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validatorResult.IsValid)
            {
                return new CreateProjectCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }
            var userByUsername = await userManager.FindByUsernameAsync(request.FullName);
            if (!userByUsername.IsSuccess)
            {
                return new CreateProjectCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { userByUsername.Error }
                };
            }
            var user = await userRepository.FindByIdAsync(Guid.Parse(userByUsername.Value.UserId));
            if (!user.IsSuccess)
            {
                return new CreateProjectCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { user.Error }
                };
            }
            var project = Project.Create(request.ProjectName, request.Description,request.GithubOwner,request.GithubToken, request.GitRepository, request.Deadline, request.FullName);
            if (!project.IsSuccess)
            {
                return new CreateProjectCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { project.Error }
                };
            }

            await projectRepository.AddAsync(project.Value);
            var assignResult = project.Value.AssignUser(user.Value);
            if (!assignResult.IsSuccess)
            {
                return new CreateProjectCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { assignResult.Error }
                };
            }
            var result = await projectRepository.UpdateAsync(project.Value);

            return new CreateProjectCommandResponse
            {
                Success = true,
                Project = new CreateProjectDto()
                {
                    ProjectId = project.Value.ProjectId,
                    ProjectName = project.Value.ProjectName,
                    Deadline = project.Value.Deadline,
                    Description = project.Value.Description,
                    GitRepository = project.Value.GitRepository,
                    FullName = project.Value.CreatedBy
                }
            };
        }
    }

}
