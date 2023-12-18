using Ergo.App.Contracts;
using Ergo.App.Services.Responses;
using Ergo.App.ViewModels;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Ergo.App.Services
{
    public class ProjectDataService : IProjectDataService
    {
        private const string RequestUri = "api/v1/Projects";
        private readonly HttpClient httpClient;
        private readonly ITokenService tokenService;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public ProjectDataService(HttpClient httpClient, ITokenService tokenService, AuthenticationStateProvider authenticationStateProvider)
        {
            this.httpClient = httpClient;
            this.tokenService = tokenService;
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
    }
}
