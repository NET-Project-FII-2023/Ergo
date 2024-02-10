using FluentValidation;

namespace Ergo.Application.Features.Badges.Commands.UpdateSpecialBadgeCommand
{
    public class UpdateSpecialBadgeCommandValidator : AbstractValidator<UpdateSpecialBadgeCommand>
    {
        public UpdateSpecialBadgeCommandValidator()
        {
            RuleFor(p => p.VotedId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.Type)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .Must(x => x == "Innovator" || x == "Quality-Master" || x == "Problem-Solver" || x == "Team-Player");
        }
    }
}
