using MediatR;
using Ergo.Application.Features.Comments.Queries.GetByTaskId;

namespace Ergo.Application.Features.Comments.Queries.GetByTaskId
{
	public class GetCommentByTaskIdQuery : IRequest<GetCommentByTaskIdQueryResponse>
	{
		public Guid TaskId { get; set; }
	}
}
