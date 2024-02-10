using Ergo.Application.Features.Users;
using Ergo.Application.Persistence;
using Ergo.Domain.Entities;
using MediatR;

namespace Ergo.Application.Features.UserPhotos.Commands.AddUserPhoto
{
    public class AddUserPhotoCommandHandler : IRequestHandler<AddUserPhotoCommand, AddUserPhotoCommandResponse>
    {
        private readonly IUserPhotoRepository userPhotoRepository;

        public AddUserPhotoCommandHandler(IUserPhotoRepository userPhotoRepository)
        {
            this.userPhotoRepository = userPhotoRepository;
        }

        public Task<AddUserPhotoCommandResponse> Handle(AddUserPhotoCommand request, CancellationToken cancellationToken)
        {
            var validator = new AddUserPhotoCommandValidator();
            var validatorResult = validator.Validate(request);
            if (!validatorResult.IsValid)
            {
                return Task.FromResult(new AddUserPhotoCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                });
            }

            var userPhoto = UserPhoto.Create(request.PhotoUrl, request.UserId);
            if (!userPhoto.IsSuccess)
            {
                return Task.FromResult(new AddUserPhotoCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { userPhoto.Error }
                });
            }

            userPhotoRepository.AddAsync(userPhoto.Value);
            return Task.FromResult(new AddUserPhotoCommandResponse
            {
                Success = true,
                UserPhoto = new UserCloudPhotoDto
                {
                    UserPhotoId = userPhoto.Value.UserPhotoId,
                    PhotoUrl = userPhoto.Value.PhotoUrl
                }
            });
        }
    }
}
