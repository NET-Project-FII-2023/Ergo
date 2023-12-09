using Ergo.App.Contracts;
using Blazored.LocalStorage;
namespace Ergo.App.Services
{
    public class TokenService : ITokenService
    {
        private const string TOKEN = "token";
        private readonly ILocalStorageService localStorageService;
        public TokenService(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;

        }
        public async Task<string> GetTokenAsync()
        {
            return await localStorageService.GetItemAsync<string>(TOKEN);
        }

        public async Task RemoveTokenAsync()
        {
            await localStorageService.RemoveItemAsync(TOKEN);
        }

        public async Task SetTokenAsync(string token)
        {
            await localStorageService.SetItemAsync(TOKEN, token);
        }
    }
}
