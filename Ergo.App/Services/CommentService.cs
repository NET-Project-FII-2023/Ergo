using Ergo.App.Contracts;
using Ergo.App.Services.Responses;
using Ergo.App.ViewModels;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
namespace Ergo.App.Services
{
    public class CommentDataService : ICommentDataService
    {
        private const string RequestUri = "api/v1/Comments";
        private readonly HttpClient httpClient;
        private readonly ITokenService tokenService;
        public CommentDataService(HttpClient httpClient, ITokenService tokenService)
        {
            this.httpClient = httpClient;
            this.tokenService = tokenService;
        }

        public async Task<ApiResponse<CommentDto>> CreateCommentAsync(CommentViewModel commentViewModel) 
        {
            httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
            var result = await httpClient.PostAsJsonAsync(RequestUri, commentViewModel);
            result.EnsureSuccessStatusCode();
            var response = await result.Content.ReadFromJsonAsync<ApiResponse<CommentDto>>();
            response!.IsSuccess = result.IsSuccessStatusCode;
            return response!;
        }


        public async Task<List<CommentViewModel>> GetCommentsAsync()
        {
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
                var result = await httpClient.GetAsync(RequestUri, HttpCompletionOption.ResponseHeadersRead);
                result.EnsureSuccessStatusCode();
                var content = await result.Content.ReadAsStringAsync();

                if (!result.IsSuccessStatusCode)
                {
                    throw new ApplicationException(content);
                }

                Console.WriteLine($"Raw JSON content: {content}");

                var comments = JsonSerializer.Deserialize<CommentItemsResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return comments?.CommentItems ?? new List<CommentViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during deserialization: {ex}");
                throw;
            }
        }
        public class CommentItemsResponse
        {
            public List<CommentViewModel>? CommentItems { get; set; }
        }
    }
}
