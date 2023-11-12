using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Features.Users.Queries.GetById
{
    public record GetByIdUserQuery(Guid Id) : IRequest<UserDto>;
}
