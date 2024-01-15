using Ergo.Application.Features.Photos.Commands.DeletePhoto;
using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.Photos.Commands.DeletePhotoCommandHandler
{
    public class DeletePhotoCommandHandler : IRequestHandler<DeletePhotoCommand, DeletePhotoCommandResponse>
    {
        private readonly IPhotoRepository photoRepository;
        public DeletePhotoCommandHandler(IPhotoRepository photoRepository)
        {
            this.photoRepository = photoRepository;
        }

        public async Task<DeletePhotoCommandResponse> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
        {
            var validator = new DeletePhotoCommandValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return new DeletePhotoCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList()
                };  
            }
            var photoToDelete = await photoRepository.FindByIdAsync(request.PhotoId);
            if(!photoToDelete.IsSuccess)
            {
                return new DeletePhotoCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Photo with the provided ID does not exist." }
                };
            }
            var result = await photoRepository.DeleteAsync(request.PhotoId);
            if(!result.IsSuccess)
            {
                return new DeletePhotoCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { result.Error }
                };
            }
            return new DeletePhotoCommandResponse
            {
                Success = true
            };
        }
    }
}
