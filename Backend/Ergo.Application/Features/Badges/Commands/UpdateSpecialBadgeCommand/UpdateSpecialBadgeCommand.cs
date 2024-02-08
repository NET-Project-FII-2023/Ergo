using MediatR;

namespace Ergo.Application.Features.Badges.Commands.UpdateSpecialBadgeCommand
{
    public class UpdateSpecialBadgeCommand : IRequest<UpdateSpecialBadgeCommandResponse>
    {
        public Guid UserId { get; set; }
        public string Type { get; set; }
    }
}
