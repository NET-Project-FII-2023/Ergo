using MediatR;

namespace Ergo.Application.Features.UserPhotos.Queries.GetUserPhoto
{
    public class GetUserPhotoQuery : IRequest<GetUserPhotoQueryResponse>
    {
        public string UserId { get; set; }
    }
}
