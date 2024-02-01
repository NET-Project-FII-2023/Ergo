using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.Photos.Queries.GetPhotosForTaskItem
{
    public class GetPhotosForTaskItemQueryHandler : IRequestHandler<GetPhotosForTaskItemQuery, GetPhotosForTaskItemQueryResponse>
    {
        private readonly IPhotoRepository photoRepository;
        private readonly ITaskItemRepository taskItemRepository;
        public GetPhotosForTaskItemQueryHandler(IPhotoRepository photoRepository, ITaskItemRepository taskItemRepository)
        {
            this.photoRepository = photoRepository;
            this.taskItemRepository = taskItemRepository;
        }

        public async Task<GetPhotosForTaskItemQueryResponse> Handle(GetPhotosForTaskItemQuery request, CancellationToken cancellationToken)
        {
            var taskItemExists = await taskItemRepository.FindByIdAsync(request.TaskItemId);
            if(!taskItemExists.IsSuccess)
            {
                return new GetPhotosForTaskItemQueryResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Task item with the provided ID does not exist." }
                };
            }
            var photos = await photoRepository.GetByTaskItemIdAsync(request.TaskItemId);
            var photoDtos = photos.Select(p => new PhotoDto
            {
                PhotoId = p.PhotoId,
                CloudURL = p.CloudURL
            }).ToList();
            return new GetPhotosForTaskItemQueryResponse
            {
                Success = true,
                Photos = photoDtos
            };
        }
    }
}
