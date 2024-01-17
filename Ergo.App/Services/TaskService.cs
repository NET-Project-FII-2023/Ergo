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

                if (tasks.TaskItems != null)
                {
                    foreach (var task in tasks.TaskItems)
                    {
                        if (task.AssignedUser == null)
                        {
                            task.AssignedUser = new TaskAssignedUserModel();
                        }
                    }
                }

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

                if (!result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<TaskDto>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return new ApiResponse<TaskDto>
                    {
                        IsSuccess = false,
                        ValidationErrors = errorResponse?.ValidationErrors,
                        Message = errorResponse?.ValidationErrors ?? "Failed to start timer: you are not assigned to this task!"
                    };
                }

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
        public class ErrorResponse
        {
            public List<string> ValidationErrors { get; set; }
        }


        public async Task<ApiResponse<TaskDto>> PauseTimerAsync(Guid taskId, Guid userId)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
                var result = await httpClient.PostAsJsonAsync($"{RequestUri}/PauseTimer", new AssignTaskItemToUserDto { TaskItemId = taskId, UserId = userId });

                // Check if the response indicates a validation error
                if (!result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<TaskDto>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return new ApiResponse<TaskDto>
                    {
                        IsSuccess = false,
                        ValidationErrors = errorResponse?.ValidationErrors,
                        Message = errorResponse?.ValidationErrors ?? "Failed to start timer: you are not assigned to this task!"
                    };
                }

                // Continue with the success path
                var response = await result.Content.ReadFromJsonAsync<ApiResponse<TaskDto>>();
                response!.IsSuccess = result.IsSuccessStatusCode;
                return response!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during timer pause: {ex}");
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

        public async Task<ApiResponse<TimeSpentDto>> GetTaskItemTimeAsync(Guid taskItemId)
        {
            try
            {
                var result = await httpClient.GetAsync($"api/v1/TaskItems/GetTaskItemTime/{taskItemId}");
                result.EnsureSuccessStatusCode();
                var content = await result.Content.ReadAsStringAsync();

                var response = JsonSerializer.Deserialize<TimeSpentDto>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return new ApiResponse<TimeSpentDto>
                {
                    IsSuccess = result.IsSuccessStatusCode,
                    Data = response
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while getting task item time: {ex}");
                throw;
            }
        }

        public async Task<ApiResponse<PhotoDto>> AddPhotoToTaskItemAsync(Guid taskItemId, Stream photoStream, string fileName)
        {
            var formData = new MultipartFormDataContent();
            using var streamContent = new StreamContent(photoStream);
            formData.Add(streamContent, "Photo", fileName);
            formData.Add(new StringContent(taskItemId.ToString()), "TaskItemId");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
            var result = await httpClient.PostAsync("api/v1/Photos", formData);
            if (!result.IsSuccessStatusCode)
            {
                var errorResponse = await result.Content.ReadAsStringAsync();
                return new ApiResponse<PhotoDto> { IsSuccess = false, Message = errorResponse };
            }

            var response = await result.Content.ReadFromJsonAsync<ApiResponse<PhotoDto>>();
            response!.IsSuccess = result.IsSuccessStatusCode;
            return response!;
        }

        public async Task<ApiResponse<List<PhotoDto>>> GetPhotosForTaskItemAsync(Guid taskItemId)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
            var result = await httpClient.GetAsync($"api/v1/Photos/{taskItemId}");

            if (!result.IsSuccessStatusCode)
            {
                var errorResponse = await result.Content.ReadAsStringAsync();
                return new ApiResponse<List<PhotoDto>> { IsSuccess = false, Message = errorResponse };
            }

            var content = await result.Content.ReadAsStringAsync();
            var photosResponse = JsonSerializer.Deserialize<PhotoResponse.PhotosResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return new ApiResponse<List<PhotoDto>>
            {
                IsSuccess = photosResponse?.Success ?? false,
                Message = photosResponse?.Message,
                Data = photosResponse?.Photos
            };
        }
    }
}
