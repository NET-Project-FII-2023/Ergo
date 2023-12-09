using Ergo.App.ViewModels;

namespace Ergo.App.Contracts
{
    public interface IAuthenticationService
    {
        Task Login(LoginViewModel loginRequest);
        Task Register(RegisterViewModel registerRequest);
        Task Logout();
    }
}
