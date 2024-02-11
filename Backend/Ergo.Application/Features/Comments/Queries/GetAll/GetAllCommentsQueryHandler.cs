using Ergo.Application.Features.Users;
using Ergo.Application.Features.Users.Queries;
using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.Comments.Queries.GetAll
{
    public class GetAllCommentsQueryHandler : IRequestHandler<GetAllCommentsQuery, GetAllCommentsResponse>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserManager _userManager;
        private readonly IUserPhotoRepository _userPhotoRepository;

        public GetAllCommentsQueryHandler(ICommentRepository commentRepository, IUserManager userManager, IUserPhotoRepository userPhotoRepository)
        {
            _commentRepository = commentRepository;
            _userPhotoRepository = userPhotoRepository;
            _userManager = userManager;
        }

        public async Task<GetAllCommentsResponse> Handle(GetAllCommentsQuery request, CancellationToken cancellationToken)
        {
            var result = await _commentRepository.GetAllAsync();

            if (!result.IsSuccess)
            {
                return new GetAllCommentsResponse
                {
                    Success = false,
                    Message = result.Error
                };
            }

            var commentsList = new List<CommentDto>();
            foreach (var comment in result.Value)
            {
                var createdBy = await _userManager.FindByUsernameAsync(comment.CreatedBy);
                if (!createdBy.IsSuccess)
                {
                    continue;
                }

                var userPhoto = await _userPhotoRepository.GetUserPhotoByUserIdAsync(createdBy.Value.UserId);
                createdBy.Value.UserPhoto = userPhoto.IsSuccess ? new UserCloudPhotoDto
                {
                    UserPhotoId = userPhoto.Value.UserPhotoId,
                    PhotoUrl = userPhoto.Value.PhotoUrl
                } : null;

                commentsList.Add(new CommentDto
                {
                    CommentId = comment.CommentId,
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
                    LastModifiedDate = comment.LastModifiedDate,
                    CommentText = comment.CommentText,
                    TaskId = comment.TaskId
                });
            }

            return new GetAllCommentsResponse
            {
                Success = true,
                Comments = commentsList
            };
        }
    }
}