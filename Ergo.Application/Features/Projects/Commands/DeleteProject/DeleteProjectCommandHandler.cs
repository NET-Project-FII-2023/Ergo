using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommandHandler :IRequestHandler<DeleteProjectCommand, DeleteProjectCommandResponse>
    {
        private readonly IProjectRepository repository;
        public DeleteProjectCommandHandler(IProjectRepository repository)
        {
            this.repository = repository;
        }

        public async Task<DeleteProjectCommandResponse> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            DeleteProjectCommandResponse response = new();
            var validator = new DeleteProjectCommandValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.ValidationsErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return response;
            }
            var projectToDelete = await repository.FindByIdAsync(request.ProjectId);

            if (projectToDelete == null)
            {
                response.Success = false;
                response.ValidationsErrors = new List<string> { "Project not found" };
                return response;
            }
            var result = await repository.DeleteAsync(request.ProjectId);
            if (!result.IsSuccess)
            {
                response.Success = false;
                response.ValidationsErrors = new List<string> { result.Error };
                return response;
            }
            return new DeleteProjectCommandResponse
            {
                Success = true
            };

        }
    }
    
}
