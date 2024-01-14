using Ergo.Domain.Common;

namespace Ergo.Domain.Entities
{
    public class Photo 
    {
        private Photo(Guid TaskItemId, byte[] ImageData)
        {
            PhotoId = Guid.NewGuid();
            this.TaskItemId = TaskItemId;
            this.ImageData = ImageData;
        }
        private Photo()
        {

        }
        public Guid PhotoId { get; set; }
        public Guid TaskItemId { get; set; }
        public byte[] ImageData { get; set; }

        public static Result<Photo> Create(Guid TaskItemId, byte[] ImageData)
        {
            if (TaskItemId == Guid.Empty)
            {
                return Result<Photo>.Failure("TaskItemId is required");
            }
            if (ImageData == null)
            {
                return Result<Photo>.Failure("Image data is required");
            }
            return Result<Photo>.Success(new Photo(TaskItemId, ImageData));
        }

    }
}
