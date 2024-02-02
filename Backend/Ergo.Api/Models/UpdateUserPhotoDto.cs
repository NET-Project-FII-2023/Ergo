namespace Ergo.Api.Models
{
    public class UpdateUserPhotoDto
    {
        public IFormFile File { get; set; }

        public string UserPhotoId { get; set; }
        public string CloudUrl { get; set; }
    }
}
