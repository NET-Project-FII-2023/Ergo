using Ergo.App.ViewModels;
namespace Ergo.App.Services.Responses
{
    public class PhotoResponse
    {
        public class PhotosResponse
        {
            public List<PhotoDto> Photos { get; set; }
            public bool Success { get; set; }
            public string? Message { get; set; } = string.Empty;
            public string? ValidationsErrors { get; set; }
        }

    }
}
