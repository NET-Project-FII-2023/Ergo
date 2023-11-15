using FluentValidation;

namespace Ergo.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            {
                RuleFor(p => p.FirstName)
                    .NotEmpty().WithMessage("{PropertyName} is required")
                    .NotNull()
                    .MaximumLength(24).WithMessage("{PropertyName} must not exceed 24 characters.");
                RuleFor(p => p.LastName)
                        .NotEmpty().WithMessage("{PropertyName} is required")
                        .NotNull()
                        .MaximumLength(24).WithMessage("{PropertyName} must not exceed 24 characters.");
                RuleFor(p => p.Email)
                        .NotEmpty().WithMessage("{PropertyName} is required")
                        .NotNull()
                        .EmailAddress().WithMessage("{PropertyName} must be a valid email address");
                RuleFor(p => p.Password)
                        .NotEmpty().WithMessage("{PropertyName} is required")
                        .NotNull()
                        .MinimumLength(8).WithMessage("{PropertyName} must be at least 8 characters")
                        .Matches("[^a-zA-Z0-9]").WithMessage("{PropertyName} must contain at least one symbol");
                RuleFor(p => p.Role)
                        .IsInEnum()
                        .WithMessage("{PropertyName} must be a valid value between 1-8");

            }
        }
    }
}