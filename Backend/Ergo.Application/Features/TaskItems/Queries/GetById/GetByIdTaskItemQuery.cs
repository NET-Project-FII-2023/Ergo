using MediatR;
using System;

namespace Ergo.Application.Features.TaskItems.Queries.GetById
{
    public class GetByIdTaskItemQuery : IRequest<GetByIdTaskItemQueryResponse>
    {
        public Guid TaskItemId { get; set; }
    }
}
