namespace Ergo.Application.Features.Photos.Queries.GetPhotosForTaskItem
{
    public class PhotoDto
    {
        public Guid PhotoId { get; set; }
        public string CloudURL { get; set; } = "";
    }

}