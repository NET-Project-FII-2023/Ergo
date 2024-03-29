﻿using Ergo.App.Contracts;
using Ergo.App.Services.Responses;
using Ergo.App.ViewModels;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Ergo.App.Services
{
    public class UserService : IUserDataService
    {
        private const string RequestUri = "api/v1/Users";
        private readonly HttpClient httpClient;
        private readonly ITokenService tokenService;

        public UserService(HttpClient httpClient, ITokenService tokenService)
        {
            this.httpClient = httpClient;
            this.tokenService = tokenService;
        }

        public async Task<UserViewModel> GetUserByEmailAsync(string email)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
                var result = await httpClient.GetAsync($"{RequestUri}/ByEmail/{email}", HttpCompletionOption.ResponseHeadersRead);
                result.EnsureSuccessStatusCode();
                var content = await result.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(content);
                var userElement = doc.RootElement.GetProperty("user");

                var user = JsonSerializer.Deserialize<UserViewModel>(userElement.GetRawText(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during deserialization: {ex}");
                throw;
            }
        }

        public async Task<string> GetUserIdByEmailAsync(string email)
        {
            try
            {
                var user = await GetUserByEmailAsync(email);
                return user.UserId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during deserialization: {ex}");
                throw;
            }
        }

        public async Task<ApiResponse<UpdateUserDto>> UpdateUserAsync(string id, UpdateUserDto updateUserDto)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
            var result = await httpClient.PutAsJsonAsync($"{RequestUri}/{id}", updateUserDto);

            result.EnsureSuccessStatusCode();
            var response = await result.Content.ReadFromJsonAsync<ApiResponse<UpdateUserDto>>();
            response!.IsSuccess = result.IsSuccessStatusCode;
            return response!;
        }

        public async Task<List<UserViewModel>> GetAssignedUsersByProjectId(Guid projectId)
        {
            try
            {
                var uri = $"{RequestUri}/ByProjectId/{projectId}";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
                var result = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                result.EnsureSuccessStatusCode();
                var content = await result.Content.ReadAsStringAsync();

                // Log or debug the content here to inspect the received JSON
                Console.WriteLine($"Received JSON: {content}");

                // Check for empty response or null content
                if (string.IsNullOrWhiteSpace(content))
                {
                    // Handle empty response
                    return new List<UserViewModel>();
                }

                // Deserialize the JSON response
                var responseObject = JsonSerializer.Deserialize<UserResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Extract the users from the response object
                var users = responseObject?.Users;

                return users ?? new List<UserViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during deserialization: {ex}");
                throw;
            }
        }

        public class UserResponse
        {
            public List<UserViewModel> Users { get; set; }
        }

    }
}
