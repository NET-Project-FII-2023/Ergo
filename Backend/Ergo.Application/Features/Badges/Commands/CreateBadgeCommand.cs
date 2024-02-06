using MediatR;

namespace Ergo.Application.Features.Badges.Commands
{
    public class CreateBadgeCommand : IRequest<CreateBadgeCommandResponse>
    { 
        public string Name { get; set; }
        public int Count { get; set; }
        public Guid UserId { get; set; }
        public string Type { get; set; }
    }
}
