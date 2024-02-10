using Ergo.Application.Features.Users;
using Ergo.Application.Responses;

namespace Ergo.Application.Features.UserPhotos.Commands.UpdateTaskPhoto
{
    public class UpdateUserPhotoCommandResponse : BaseResponse
    {
        public UpdateUserPhotoCommandResponse() : base()
        {
        }

        public UserCloudPhotoDto UserPhoto { get; set; }
    }
}