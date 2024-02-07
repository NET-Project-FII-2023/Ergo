using MediatR;

namespace Ergo.Application.Features.UserPhotos.Commands.UpdateTaskPhoto
{
    public class UpdateUserPhotoCommand : IRequest<UpdateUserPhotoCommandResponse>
    {
        public string UserPhotoId { get; set; }
        public string PhotoUrl { get; set; }
    }
}
