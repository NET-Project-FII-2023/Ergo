using MediatR;

namespace Ergo.Application.Features.Users.Queries.GetById
{
    public record GetByIdUserQuery(Guid UserId) : IRequest<UserDto>;
}
