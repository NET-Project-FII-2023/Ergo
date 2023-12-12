using Ergo.Application.Responses;

namespace Ergo.Application.Features.Comments.Queries.GetByTaskId
{
	public class GetCommentByTaskIdQueryResponse : BaseResponse
	{
		public List<CommentDto> Comments { get; set; }
	}
}
