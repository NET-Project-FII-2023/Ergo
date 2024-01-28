namespace Ergo.App.Contracts
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync();
        Task RemoveTokenAsync();
        Task SetTokenAsync(string token);
        Task<string> DecodeEmailFromTokenAsync(string token);
        Task<string> DecodeUsernameFromTokenAsync(string token);
    }
}
