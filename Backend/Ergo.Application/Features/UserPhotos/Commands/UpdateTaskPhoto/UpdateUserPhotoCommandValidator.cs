using FluentValidation;

namespace Ergo.Application.Features.UserPhotos.Commands.UpdateTaskPhoto
{
    public class UpdateUserPhotoCommandValidator : AbstractValidator<UpdateUserPhotoCommand>
    {
        public UpdateUserPhotoCommandValidator()
        {
            RuleFor(p => p.UserPhotoId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.PhotoUrl)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}
