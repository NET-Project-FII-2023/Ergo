using FluentValidation;

namespace Ergo.Application.Features.UserPhotos.Commands.AddUserPhoto
{
    public class AddUserPhotoCommandValidator : AbstractValidator<AddUserPhotoCommand>
    {
        public AddUserPhotoCommandValidator()
        {
            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("UserId is required.");
            RuleFor(p => p.PhotoUrl)
                .NotEmpty().WithMessage("PhotoUrl is required.");
        }
    }
}
