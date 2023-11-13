using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Ergo.Application.Features.TaskItems.Queries.GetById
{
    public record GetByIdTaskItemQuery(Guid Id) : IRequest<TaskItemDto>;
}
