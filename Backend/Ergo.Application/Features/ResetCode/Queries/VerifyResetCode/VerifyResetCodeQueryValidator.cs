using FluentValidation;

namespace Ergo.Application.Features.ResetCode.Queries.VerifyResetCode
{
    public class VerifyResetCodeQueryValidator : AbstractValidator<VerifyResetCodeQuery>
    {
        public VerifyResetCodeQueryValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .EmailAddress().WithMessage("{PropertyName} is not a valid email address.");
            RuleFor(p => p.Code)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
