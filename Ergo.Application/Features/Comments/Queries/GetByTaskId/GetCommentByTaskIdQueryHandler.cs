using Ergo.Application.Responses;
using MediatR;
using Ergo.Application.Features.Comments.Queries.GetByTaskId;
using Ergo.Application.Persistence;

namespace Ergo.Application.Features.Comments.Queries.GetByTaskId
{
	public class GetCommentByTaskIdQueryHandler : IRequestHandler<GetCommentByTaskIdQuery,GetCommentByTaskIdQueryResponse>
	{
		private readonly ICommentRepository _commentRepository;

		public GetCommentByTaskIdQueryHandler(ICommentRepository commentRepository)
		{
			_commentRepository = commentRepository;
		}

		public async Task<GetCommentByTaskIdQueryResponse> Handle(GetCommentByTaskIdQuery request, CancellationToken cancellationToken)
		{
			var commentResult = await _commentRepository.GetCommentByTaskIdAsync(request.TaskId);

			if (!commentResult.IsSuccess)
			{
				return new GetCommentByTaskIdQueryResponse
				{
					Success = false,
					Comments = null
				};
			}

			var comments = commentResult.Value;

			var commentDtos = new List<CommentDto>();

			foreach (var comment in comments)
			{
				commentDtos.Add(new CommentDto
				{
					CommentId = comment.CommentId,
					TaskId = comment.TaskId,
					CommentText = comment.CommentText,
					CreatedBy = comment.CreatedBy,
					CreatedDate = comment.CreatedDate,
					LastModifiedBy = comment.LastModifiedBy,
					LastModifiedDate = comment.LastModifiedDate

				});
			}

			return new GetCommentByTaskIdQueryResponse
			{
				Success = true,
				Comments = commentDtos
			};
		}
	}
}
