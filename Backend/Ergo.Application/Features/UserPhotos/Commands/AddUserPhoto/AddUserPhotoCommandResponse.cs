using Ergo.Application.Features.Users;
using Ergo.Application.Responses;

namespace Ergo.Application.Features.UserPhotos.Commands.AddUserPhoto
{
    public class AddUserPhotoCommandResponse : BaseResponse
    {
        public AddUserPhotoCommandResponse() : base()
        {
        }

        public UserCloudPhotoDto UserPhoto { get; set; }
    }
}