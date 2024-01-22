using Ergo.App.Contracts;
using Ergo.App.Services.Responses;
using Ergo.App.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ergo.App.Services
{
    public class MachineLearningService : IMachineLearningService
    {
        private const string RequestUri = "api/v1/ML";
        private readonly HttpClient httpClient;
        private readonly ITokenService tokenService;

        public MachineLearningService(HttpClient httpClient, ITokenService tokenService)
        {
            this.httpClient = httpClient;
            this.tokenService = tokenService;
        }

        public async Task<ApiResponse<PredictionDto>> GetPredictionAsync(TaskPredictionViewModel taskPredictionViewModel)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());

                var result = await httpClient.PostAsJsonAsync(RequestUri, taskPredictionViewModel);
                result.EnsureSuccessStatusCode();

                var content = await result.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<PredictionDto>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return new ApiResponse<PredictionDto>
                {
                    IsSuccess = result.IsSuccessStatusCode,
                    Data = response
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during prediction: {ex}");
                throw;
            }
        }
    }
}
