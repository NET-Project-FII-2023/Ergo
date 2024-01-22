using Ergo.Application.Responses;

namespace Ergo.Application.Features.Photos.Queries.GetPhotosForTaskItem
{
    public class GetPhotosForTaskItemQueryResponse : BaseResponse
    {
        public GetPhotosForTaskItemQueryResponse() : base()
        {
        }
        public List<PhotoDto> Photos { get; set; }
    }
}