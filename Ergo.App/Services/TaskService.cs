using Ergo.App.Contracts;
using Ergo.App.Services.Responses;
using Ergo.App.ViewModels;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Ergo.App.Services
{
    public class TaskDataService : ITaskDataService
    {
        private const string RequestUri = "api/v1/TaskItems";
        private readonly HttpClient httpClient;
        private readonly ITokenService tokenService;

        public TaskDataService(HttpClient httpClient, ITokenService tokenService)
        {
            this.httpClient = httpClient;
            this.tokenService = tokenService;
        }

        public async Task<ApiResponse<TaskDto>> CreateTaskAsync(TaskViewModel taskViewModel)
        {
            httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
            var result = await httpClient.PostAsJsonAsync(RequestUri, taskViewModel);
            result.EnsureSuccessStatusCode();
            var response = await result.Content.ReadFromJsonAsync<ApiResponse<TaskDto>>();
            response!.IsSuccess = result.IsSuccessStatusCode;
            return response!;
        }

        public async Task<List<TaskViewModel>> GetTasksAsync()
        {
            var result = await httpClient.GetAsync(RequestUri, HttpCompletionOption.ResponseHeadersRead);
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var tasks = JsonSerializer.Deserialize<List<TaskViewModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return tasks!;
        }
    }
}
