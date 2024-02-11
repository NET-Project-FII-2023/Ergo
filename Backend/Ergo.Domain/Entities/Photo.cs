using Ergo.Domain.Common;

namespace Ergo.Domain.Entities
{
    public class Photo 
    {
        private Photo(Guid TaskItemId , string cloudUrl)
        {
            PhotoId = Guid.NewGuid();
            this.TaskItemId = TaskItemId;
            CloudURL = cloudUrl;
        }
        public Photo()
        {

        }
        public Guid PhotoId { get; set; }
        public Guid TaskItemId { get; set; }
        public string CloudURL { get; set; }

        public static Result<Photo> Create(Guid TaskItemId, string cloudUrl)
        {
            if (TaskItemId == Guid.Empty)
            {
                return Result<Photo>.Failure("TaskItemId is required");
            }
            if (string.IsNullOrEmpty(cloudUrl))
            {
                return Result<Photo>.Failure("CloudURL is required");
            }
            

            return Result<Photo>.Success(new Photo(TaskItemId, cloudUrl));
        }

    }
}
