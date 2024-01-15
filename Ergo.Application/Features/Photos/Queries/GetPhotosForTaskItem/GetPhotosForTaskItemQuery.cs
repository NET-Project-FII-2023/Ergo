using MediatR;

namespace Ergo.Application.Features.Photos.Queries.GetPhotosForTaskItem
{
    public class GetPhotosForTaskItemQuery : IRequest<GetPhotosForTaskItemQueryResponse>
    {
        public Guid TaskItemId { get; set; }
    }
}
