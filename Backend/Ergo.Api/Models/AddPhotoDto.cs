namespace Ergo.Api.Models
{
    public class AddPhotoDto
    {
        public IFormFile File { get; set; }
        public Guid TaskItemId { get; set; }
    }
}
