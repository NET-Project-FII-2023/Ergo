using System.Net.Http.Headers;
using System.Text.Json;
using Ergo.App.Contracts;
using Ergo.App.ViewModels;

namespace Ergo.App.Services;

public class InboxItemService : IInboxItemDataService
{
    private const string RequestUri = "api/v1/InboxItem";
    private readonly HttpClient httpClient;
    private readonly ITokenService tokenService;
    private readonly IUserDataService userDataService;
    
    public InboxItemService(HttpClient httpClient, ITokenService tokenService, IUserDataService userDataService)
    {
        this.httpClient = httpClient;
        this.tokenService = tokenService;
        this.userDataService = userDataService;
    }
    
    public class InboxItemsResponse
    {
        public List<InboxItemViewModel>? InboxItems { get; set; }
    }
    
    public async Task<List<InboxItemViewModel>> GetInboxItemsByUserIdFromTokenAsync()
    {
        try
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
            var email = await tokenService.DecodeEmailFromTokenAsync(await tokenService.GetTokenAsync());
            var user = await userDataService.GetUserByEmailAsync(email!);
            
            var result = await httpClient.GetAsync($"{RequestUri}/{user.UserId}", HttpCompletionOption.ResponseHeadersRead);
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync();
            
            if (!result.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var inboxItems = JsonSerializer.Deserialize<InboxItemsResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            return inboxItems?.InboxItems ?? new List<InboxItemViewModel>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during deserialization: {ex}");
            throw;
        }
    }
}