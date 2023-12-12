using Ergo.App.Contracts;
using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
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

        public Task<string> DecodeEmailFromTokenAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);
            var email = jsonToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            return Task.FromResult(email);
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
