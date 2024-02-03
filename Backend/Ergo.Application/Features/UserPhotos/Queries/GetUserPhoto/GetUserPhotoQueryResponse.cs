using Ergo.Application.Responses;

namespace Ergo.Application.Features.UserPhotos.Queries.GetUserPhoto
{
    public class GetUserPhotoQueryResponse : BaseResponse
    {
        public GetUserPhotoQueryResponse() : base()
        {
        }
        public string UserPhotoUrl { get; set; }

    }
}