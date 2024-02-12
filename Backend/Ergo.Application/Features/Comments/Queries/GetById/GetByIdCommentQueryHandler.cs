using Ergo.Application.Features.Users;
using Ergo.Application.Features.Users.Queries;
using Ergo.Application.Persistence;
using MediatR;


namespace Ergo.Application.Features.Comments.Queries.GetById
{
    public class GetByIdCommentQueryHandler : IRequestHandler <GetByIdCommentQuery,GetByIdCommentQueryResponse>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserManager _userManager;
        private readonly IUserPhotoRepository _userPhotoRepository;
        
        public GetByIdCommentQueryHandler(ICommentRepository commentRepository, IUserManager userManager, IUserPhotoRepository userPhotoRepository)
        {
            _commentRepository = commentRepository;
            _userPhotoRepository = userPhotoRepository;
            _userManager = userManager;
        }

        public async Task<GetByIdCommentQueryResponse> Handle(GetByIdCommentQuery request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.FindByIdAsync(request.CommentId);

            if(!comment.IsSuccess)
            {
                return new GetByIdCommentQueryResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { comment.Error }
                };
            }

            var createdBy = await _userManager.FindByUsernameAsync(comment.Value.CreatedBy);
            if (!createdBy.IsSuccess)
            {
                return new GetByIdCommentQueryResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { createdBy.Error }
                };
            }

            var userPhoto = await _userPhotoRepository.GetUserPhotoByUserIdAsync(createdBy.Value.UserId);
            createdBy.Value.UserPhoto = userPhoto.IsSuccess ? new UserCloudPhotoDto
            {
                UserPhotoId = userPhoto.Value.UserPhotoId,
                PhotoUrl = userPhoto.Value.PhotoUrl
            } : null;

            return new GetByIdCommentQueryResponse
            {
                Success = true,
                Comment = new CommentDto
                {
                    CommentId = comment.Value.CommentId,
                    CommentText = comment.Value.CommentText,
                    CreatedBy = new UserCommentDto
                    {
                        UserId = createdBy.Value.UserId,
                        Username = createdBy.Value.Username,
                        Name = createdBy.Value.Name,
                        Email = createdBy.Value.Email,
                        UserPhoto = createdBy.Value.UserPhoto
                    },
                    CreatedDate = comment.Value.CreatedDate,
                    LastModifiedBy = comment.Value.LastModifiedBy,
                    LastModifiedDate = comment.Value.LastModifiedDate,
                    TaskId = comment.Value.TaskId
                }
            };
           
        }
    }
}
