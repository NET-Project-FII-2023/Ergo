using MediatR;

namespace Ergo.Application.Features.ResetCode.Queries.VerifyResetCode
{
    public class VerifyResetCodeQuery : IRequest<VerifyResetCodeResponse>
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
