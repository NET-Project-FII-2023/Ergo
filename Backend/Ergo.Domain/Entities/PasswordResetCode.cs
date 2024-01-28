using Ergo.Domain.Common;

namespace Ergo.Domain.Entities
{
    public class PasswordResetCode
    {
        private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(5);
        private PasswordResetCode(string email, string code)
        {
            PasswordResetCodeId = Guid.NewGuid();
            Email = email;
            Code = code;
            ExpirationTime = DateTime.UtcNow.Add(DefaultExpiration);
            CreatedAt = DateTime.UtcNow;
        }
        private PasswordResetCode()
        {

        }
        public Guid PasswordResetCodeId { get; private set; }
        public string Email { get; private set; }
        public string Code { get; private set; }
        public DateTime ExpirationTime { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public static Result<PasswordResetCode> Create(string email, string code)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Result<PasswordResetCode>.Failure("Email cannot be empty or whitespace");
            }
            

            if (string.IsNullOrWhiteSpace(code))
            {
                return Result<PasswordResetCode>.Failure("Code cannot be empty or whitespace");
            }

            return Result<PasswordResetCode>.Success(new PasswordResetCode(email, code));
        }

        public bool IsValid()
        {
            return DateTime.UtcNow <= ExpirationTime;
        }

        public void Invalidate()
        {
            ExpirationTime = DateTime.UtcNow;
        }
    }
}
