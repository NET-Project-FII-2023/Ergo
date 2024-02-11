using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.Comments.Queries.GetAll
{
    public class GetAllCommentsQueryHandler :IRequestHandler<GetAllCommentsQuery, GetAllCommentsResponse>
    {
        private readonly ICommentRepository commentRepository;

        public GetAllCommentsQueryHandler(ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        public async Task<GetAllCommentsResponse> Handle(GetAllCommentsQuery request,CancellationToken cancellationToken)
        {
            GetAllCommentsResponse response = new();
            var result = await commentRepository.GetAllAsync();
            if (result.IsSuccess)
            {
                response.Comments = result.Value.Select(u => new CommentDto{
                    CommentId = u.CommentId,
                    CreatedBy = u.CreatedBy,
                    CreatedDate = u.CreatedDate,
                    LastModifiedBy = u.LastModifiedBy,
                    LastModifiedDate = u.LastModifiedDate,
                    CommentText = u.CommentText,
                    TaskId = u.TaskId
                }).ToList();
            }
            return response;
        }

    }
}
