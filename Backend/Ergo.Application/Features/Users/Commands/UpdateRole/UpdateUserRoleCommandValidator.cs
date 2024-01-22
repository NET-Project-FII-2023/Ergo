using FluentValidation;

namespace Ergo.Application.Features.Users.Commands.UpdateRole
{
    public class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserRoleCommand>
    {
        public UpdateUserRoleCommandValidator()
        {
            RuleFor(u => u.UserId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(u => u.Role)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .Must(role => role == "Admin" || role == "User")
                .WithMessage("{PropertyName} must be Admin or User");
        }
    }
}
