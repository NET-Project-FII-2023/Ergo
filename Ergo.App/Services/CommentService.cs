using Ergo.App.Contracts;
using Ergo.App.Services.Responses;
using Ergo.App.ViewModels;
using Ergo.Domain.Entities;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using static Ergo.App.Services.TaskDataService;
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

                var comments = JsonSerializer.Deserialize<CommentsResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return comments?.Comments ?? new List<CommentViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during deserialization: {ex}");
                throw;
            }
        }

        public async Task<List<CommentViewModel>> GetCommentsByTaskIdAsync(Guid taskId)
        {
            try
            {
                var uri = $"{RequestUri}/ByTaskId/{taskId}";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
                var result = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                result.EnsureSuccessStatusCode();
                var content = await result.Content.ReadAsStringAsync();

                if (!result.IsSuccessStatusCode)
                {
                    throw new ApplicationException(content);
                }

                var comments = JsonSerializer.Deserialize<CommentsResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

				if (comments?.Comments != null)
				{
                    Console.WriteLine($"Received {comments.Comments.Count} comments");
                    foreach (var comment in comments.Comments)
					{
						Console.WriteLine($"Received Comment: {comment.CommentId}");
					}
				}
                else
                {
                    Console.WriteLine($"Received 0 comments");
                }


				return comments?.Comments ?? new List<CommentViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during deserialization: {ex}");
                throw;
            }
        }
        
        public async Task<ApiResponse<UpdateCommentDto>> UpdateCommentAsync(UpdateCommentDto updateCommentDto)
        {
            httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
            //debug print request uri
            Console.WriteLine($"Request URI: {RequestUri}/{updateCommentDto.CommentId}");
            var result = await httpClient.PutAsJsonAsync($"{RequestUri}/{updateCommentDto.CommentId}", updateCommentDto);
            result.EnsureSuccessStatusCode();
            var response = await result.Content.ReadFromJsonAsync<ApiResponse<UpdateCommentDto>>();
            response!.IsSuccess = result.IsSuccessStatusCode;
            return response!;
        }
        
        public async Task<ApiResponse<CommentDto>> DeleteCommentAsync(Guid commentId)
        {
            httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
            var result = await httpClient.DeleteAsync($"{RequestUri}/{commentId}");
            result.EnsureSuccessStatusCode();
            var response = await result.Content.ReadFromJsonAsync<ApiResponse<CommentDto>>();
            response!.IsSuccess = result.IsSuccessStatusCode;
            return response!;
        }

        public class CommentsResponse
        {
            public List<CommentViewModel>? Comments { get; set; }
        }
    }
}
