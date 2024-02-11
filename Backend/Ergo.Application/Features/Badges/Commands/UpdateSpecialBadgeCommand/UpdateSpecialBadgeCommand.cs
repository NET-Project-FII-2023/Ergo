using MediatR;

namespace Ergo.Application.Features.Badges.Commands.UpdateSpecialBadgeCommand
{
    public class UpdateSpecialBadgeCommand : IRequest<UpdateSpecialBadgeCommandResponse>
    {
        public Guid VoterId { get; set; }
        public Guid VotedId { get; set; }
        public string Type { get; set; }
    }
}
