using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.UserPhotos.Commands.UpdateTaskPhoto
{
    public class UpdateUserPhotoCommandHandler : IRequestHandler<UpdateUserPhotoCommand, UpdateUserPhotoCommandResponse>
    {
        private readonly IUserPhotoRepository userPhotoRepository;
        public UpdateUserPhotoCommandHandler(IUserPhotoRepository userPhotoRepository)
        {
            this.userPhotoRepository = userPhotoRepository;
        }
        public async Task<UpdateUserPhotoCommandResponse> Handle(UpdateUserPhotoCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateUserPhotoCommandValidator();
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validatorResult.IsValid)
            {
                return new UpdateUserPhotoCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }
            var userPhoto = await userPhotoRepository.FindByIdAsync(Guid.Parse(request.UserPhotoId));
            if (!userPhoto.IsSuccess)
            {
                return new UpdateUserPhotoCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { userPhoto.Error }
                };
            }
            var updateResult = userPhoto.Value.UpdatePhoto(request.PhotoUrl);
            if (!updateResult.IsSuccess)
            {
                return new UpdateUserPhotoCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { updateResult.Error }
                };
            }
            await userPhotoRepository.UpdateAsync(userPhoto.Value);
            return new UpdateUserPhotoCommandResponse
            {
                Success = true
            };
        }

    }
}
