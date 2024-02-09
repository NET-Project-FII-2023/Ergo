using Ergo.Application.Persistence;
using Ergo.Domain.Entities;
using MediatR;

namespace Ergo.Application.Features.TaskItems.Commands.CreateTaskItem
{
    public class CreateTaskItemCommandHandler : IRequestHandler<CreateTaskItemCommand, CreateTaskItemCommandResponse>
    {

        private readonly ITaskItemRepository taskItemRepository;
        private readonly IProjectRepository projectRepository;

        public CreateTaskItemCommandHandler(ITaskItemRepository taskItemRepository, IProjectRepository projectRepository)
        {
            this.taskItemRepository = taskItemRepository;
            this.projectRepository = projectRepository;
        }

        public async Task<CreateTaskItemCommandResponse> Handle(CreateTaskItemCommand request,
            CancellationToken cancellationToken)
        {
            var validator = new CreateTaskItemCommandValidator();
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validatorResult.IsValid)
            {
                return new CreateTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }
            var projectExists = await projectRepository.ProjectExists(request.ProjectId);
            if (!projectExists)
            {
                return new CreateTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Project with the provided ID does not exist." }
                };
            }

            var taskItem = TaskItem.Create(request.TaskName, request.Description, request.Deadline, request.CreatedBy, request.ProjectId, request.Branch);
            if (!taskItem.IsSuccess)
            {
                return new CreateTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { taskItem.Error }
                };
            }
            await taskItemRepository.AddAsync(taskItem.Value);
            return new CreateTaskItemCommandResponse
            {
                Success = true,
                TaskItem = new CreateTaskItemDto
                {
                    TaskName = taskItem.Value.TaskName,
                    Description = taskItem.Value.Description,
                    Deadline = taskItem.Value.Deadline,
                    ProjectId = taskItem.Value.ProjectId,
                    CreatedBy = taskItem.Value.CreatedBy,
                    State = taskItem.Value.State,
                    Branch = taskItem.Value.Branch
                }
            };

        }
    }
}
