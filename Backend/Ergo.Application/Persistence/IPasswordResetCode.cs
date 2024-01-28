using Ergo.Domain.Entities;

namespace Ergo.Application.Persistence
{
    public interface IPasswordResetCode : IAsyncRepository<PasswordResetCode>
    {
        Task<bool> HasValidCodeByEmailAsync(string email,string code);
        Task InvalidateExistingCodesAsync(string email);
    }
}
