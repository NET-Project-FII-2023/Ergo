using Ergo.App.Contracts;
using Ergo.App.Services.Responses;
using Ergo.App.ViewModels;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using static Ergo.App.Services.TaskDataService;

namespace Ergo.App.Services
{
    public class ProjectDataService : IProjectDataService
    {
        private const string RequestUri = "api/v1/Projects";
        private readonly HttpClient httpClient;
        private readonly ITokenService tokenService;
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly IUserDataService userDataService;

        public ProjectDataService(HttpClient httpClient, ITokenService tokenService, IUserDataService userDataService, AuthenticationStateProvider authenticationStateProvider)
        {
            this.httpClient = httpClient;
            this.tokenService = tokenService;
            this.userDataService = userDataService;
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<List<ProjectViewModel>> GetProjectsAsync()
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

                var projects = JsonSerializer.Deserialize<ProjectsResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return projects?.Projects ?? new List<ProjectViewModel>();
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Unauthorized access.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public class ProjectsResponse
        {
            public List<ProjectViewModel> Projects { get; set; }
        }

        public async Task<ApiResponse<ProjectDto>> CreateProjectAsync(ProjectViewModel projectViewModel)
        {
            httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());

            var username = await tokenService.DecodeUsernameFromTokenAsync(await tokenService.GetTokenAsync());
            if (username != null)
            {
                projectViewModel.FullName = username;
            }

            var result = await httpClient.PostAsJsonAsync(RequestUri, projectViewModel);
            result.EnsureSuccessStatusCode();
            var response = await result.Content.ReadFromJsonAsync<ApiResponse<ProjectDto>>();
            response!.IsSuccess = result.IsSuccessStatusCode;
            return response!;
        }

        public async Task<List<ProjectViewModel>> GetProjectsByUserIdFromTokenAsync()
        {
            try
            {
                httpClient.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
                var email = await tokenService.DecodeEmailFromTokenAsync(await tokenService.GetTokenAsync());
                var user = await userDataService.GetUserByEmailAsync(email!);
                var result = await httpClient.GetAsync($"{RequestUri}/GetProjectsByUserId/{user.UserId}", HttpCompletionOption.ResponseHeadersRead);
                result.EnsureSuccessStatusCode();
                var content = await result.Content.ReadAsStringAsync();

                if (!result.IsSuccessStatusCode)
                {
                    throw new ApplicationException(content);
                }
                var projects = JsonSerializer.Deserialize<ProjectsResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return projects?.Projects ?? new List<ProjectViewModel>();
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Unauthorized access.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during deserialization: {ex}");
                throw;
            }

        }


        public async Task<ApiResponse<ProjectDto>> AssignUserToProjectAsync(ProjectAssignUserViewModel projectAssignUserViewModel)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
                AssignUserToProjectDto assignUserToProjectDto = new()
                {
                    ProjectId = projectAssignUserViewModel.ProjectId,
                    UserId = projectAssignUserViewModel.UserId
                };

                var result = await httpClient.PostAsJsonAsync($"{RequestUri}/AssignUserToProject", assignUserToProjectDto);
                if (!result.IsSuccessStatusCode)
                {
                    if (result.StatusCode == HttpStatusCode.NotFound)
                    {
                        Console.WriteLine("User not found.");
                        return null;
                    }
                    else
                    {
                        throw new HttpRequestException($"Unexpected HTTP status code {result.StatusCode}");
                    }
                }
                var response = await result.Content.ReadFromJsonAsync<ApiResponse<ProjectDto>>();
                response!.IsSuccess = result.IsSuccessStatusCode;
                return response!;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
                throw;
            }
        }


        public async Task<List<ProjectViewModel>> GetProjectsByUserIdAsync(string userId)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());

                var result = await httpClient.GetAsync($"{RequestUri}/GetProjectsByUserId/{userId}", HttpCompletionOption.ResponseHeadersRead);
                result.EnsureSuccessStatusCode();
                var content = await result.Content.ReadAsStringAsync();

                if (!result.IsSuccessStatusCode)
                {
                    throw new ApplicationException(content);
                }
                var projects = JsonSerializer.Deserialize<ProjectsResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return projects?.Projects ?? new List<ProjectViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during deserialization: {ex}");
                throw;
            }
        }
    }
}
