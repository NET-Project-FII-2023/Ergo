using MediatR;
namespace Ergo.Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectCommand : IRequest<CreateProjectCommandResponse>
    {
        public string ProjectName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public DateTime Deadline { get; set; }
    }
}

