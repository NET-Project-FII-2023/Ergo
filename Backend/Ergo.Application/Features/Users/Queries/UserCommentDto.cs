namespace Ergo.Application.Features.Users.Queries
{
    public class UserCommentDto
    {
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public UserCloudPhotoDto? UserPhoto { get; set; }
    }
}
