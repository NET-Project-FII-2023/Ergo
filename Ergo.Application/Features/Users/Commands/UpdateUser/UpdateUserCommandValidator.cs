using FluentValidation;

namespace Ergo.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required")
                .NotNull().WithMessage("Id is required");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .NotNull().WithMessage("Username is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .NotNull().WithMessage("Name is required");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .NotNull().WithMessage("Email is required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(7).WithMessage("{PropertyName} must be at least 7 characters")
                .Matches("[^a-zA-Z0-9]").WithMessage("{PropertyName} must contain at least one")
                .NotNull().WithMessage("Password is required");
        }
    }
}
