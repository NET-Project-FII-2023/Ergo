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

        public async Task<string> GetUsernameFromTokenAsync()
        {
            try
            {
                var token = await tokenService.GetTokenAsync();
                return await tokenService.DecodeUsernameFromTokenAsync(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while fetching email from token: {ex}");
                throw;
            }
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

                var tasks = JsonSerializer.Deserialize<TaskItemsResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return tasks?.TaskItems ?? new List<TaskViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during deserialization: {ex}");
                throw;
            }
        }

        public async Task<List<TaskViewModel>> GetTasksByProjectIdAsync(Guid projectId)
        {
            try
            {
                var uri = $"{RequestUri}/ByProject/{projectId}";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
                var result = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                result.EnsureSuccessStatusCode();
                var content = await result.Content.ReadAsStringAsync();

                if (!result.IsSuccessStatusCode)
                {
                    throw new ApplicationException(content);
                }

                var tasks = JsonSerializer.Deserialize<TaskItemsResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return tasks?.TaskItems ?? new List<TaskViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during deserialization: {ex}");
                throw;
            }
        }
        
        public async Task<ApiResponse<UpdateTaskDto>> UpdateTaskAsync(UpdateTaskDto updateTaskDto)
        {
            httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
            var result = await httpClient.PutAsJsonAsync($"{RequestUri}/{updateTaskDto.TaskItemId}", updateTaskDto);
            result.EnsureSuccessStatusCode();
            var response = await result.Content.ReadFromJsonAsync<ApiResponse<UpdateTaskDto>>();
            response!.IsSuccess = result.IsSuccessStatusCode;
            return response!;
        }


        public async Task<ApiResponse<TaskDto>> StartTimerAsync(Guid taskId, Guid userId)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
                var result = await httpClient.PostAsJsonAsync($"{RequestUri}/StartTimer", new AssignTaskItemToUserDto { TaskItemId = taskId, UserId = userId });
                result.EnsureSuccessStatusCode();
                var response = await result.Content.ReadFromJsonAsync<ApiResponse<TaskDto>>();
                response!.IsSuccess = result.IsSuccessStatusCode;
                return response!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during timer start: {ex}");
                throw;
            }
        }

        public async Task<ApiResponse<TaskDto>> PauseTimerAsync(Guid taskId, Guid userId)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
                var result = await httpClient.PostAsJsonAsync($"{RequestUri}/PauseTimer", new AssignTaskItemToUserDto { TaskItemId = taskId, UserId = userId });
                result.EnsureSuccessStatusCode();
                var response = await result.Content.ReadFromJsonAsync<ApiResponse<TaskDto>>();
                response!.IsSuccess = result.IsSuccessStatusCode;
                return response!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during timer start: {ex}");
                throw;
            }
        }

        public class TaskItemsResponse
        {
            public List<TaskViewModel> TaskItems { get; set; }
        }

        public async Task<ApiResponse<TaskDto>> AssignUserToTaskAsync(Guid taskId, Guid userId)
        {
            httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
            var result = await httpClient.PostAsJsonAsync($"{RequestUri}/AssignUser", new AssignTaskItemToUserDto { TaskItemId = taskId, UserId = userId });
            result.EnsureSuccessStatusCode();
            var response = await result.Content.ReadFromJsonAsync<ApiResponse<TaskDto>>();
            Console.WriteLine(response);
            response!.IsSuccess = result.IsSuccessStatusCode;
            return response!;
        }


    }
}
