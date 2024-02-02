namespace Ergo.Api.Models
{
    public class AddUserPhotoDto
    {
        public IFormFile File { get; set; }
        public string UserId { get; set; }
    }
}
