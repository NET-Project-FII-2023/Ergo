using MediatR;
using Ergo.Application.Features.Users;
using Ergo.Application.Features.Users.Queries;
using Ergo.Application.Persistence;

namespace Ergo.Application.Features.Comments.Queries.GetByTaskId
{
	public class GetCommentByTaskIdQueryHandler : IRequestHandler<GetCommentByTaskIdQuery,GetCommentByTaskIdQueryResponse>
	{
		private readonly ICommentRepository _commentRepository;
		private readonly IUserManager _userManager;
		private readonly IUserPhotoRepository _userPhotoRepository;

		public GetCommentByTaskIdQueryHandler(ICommentRepository commentRepository, IUserManager userManager, IUserPhotoRepository userPhotoRepository)
		{
			_commentRepository = commentRepository;
			_userPhotoRepository = userPhotoRepository;
			_userManager = userManager;
		}

		public async Task<GetCommentByTaskIdQueryResponse> Handle(GetCommentByTaskIdQuery request, CancellationToken cancellationToken)
		{
			var commentResult = await _commentRepository.GetCommentByTaskIdAsync(request.TaskId);

			if (!commentResult.IsSuccess)
			{
				return new GetCommentByTaskIdQueryResponse
				{
					Success = false,
					Message = commentResult.Error
				};
			}

			var comments = commentResult.Value;
			var commentDtos = new List<CommentDto>();
			foreach (var comment in comments)
			{
				var createdBy = await _userManager.FindByUsernameAsync(comment.CreatedBy);

				if (createdBy.IsSuccess)
				{
					var userPhoto = await _userPhotoRepository.GetUserPhotoByUserIdAsync(createdBy.Value.UserId);
					createdBy.Value.UserPhoto = userPhoto.IsSuccess ? new UserCloudPhotoDto
					{
						UserPhotoId = userPhoto.Value.UserPhotoId,
						PhotoUrl = userPhoto.Value.PhotoUrl
					} : null;

					commentDtos.Add(new CommentDto
					{
						CommentId = comment.CommentId,
						TaskId = comment.TaskId,
						CommentText = comment.CommentText,
						CreatedBy = new UserCommentDto
						{
							UserId = createdBy.Value.UserId,
							Username = createdBy.Value.Username,
							Name = createdBy.Value.Name,
							Email = createdBy.Value.Email,
							UserPhoto = createdBy.Value.UserPhoto
						},
						CreatedDate = comment.CreatedDate,
						LastModifiedBy = comment.LastModifiedBy,
						LastModifiedDate = comment.LastModifiedDate

					});
				}
			}

			return new GetCommentByTaskIdQueryResponse
			{
				Success = true,
				Comments = commentDtos
			};
		}
	}
}
