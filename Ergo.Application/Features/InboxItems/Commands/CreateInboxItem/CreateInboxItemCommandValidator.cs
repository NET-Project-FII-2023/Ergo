using FluentValidation;

namespace Ergo.Application.Features.InboxItems.Commands.CreateInboxItem
{
    public class CreateInboxItemCommandValidator : AbstractValidator<CreateInboxItemCommand>
    {
        public CreateInboxItemCommandValidator()
        {
            RuleFor(i => i.UserId).NotEmpty().NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();
            RuleFor(i => i.Message).NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();
        }
    }
}
