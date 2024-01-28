using Ergo.Application.Persistence;
using MediatR;


namespace Ergo.Application.Features.Comments.Queries.GetById
{
    public class GetByIdCommentQueryHandler : IRequestHandler <GetByIdCommentQuery,GetByIdCommentQueryResponse>
    {
        private readonly ICommentRepository commentRepository;
        
        public GetByIdCommentQueryHandler(ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        public async Task<GetByIdCommentQueryResponse> Handle(GetByIdCommentQuery request, CancellationToken cancellationToken)
        {
            GetByIdCommentQueryResponse response = new();
            var comment = await commentRepository.FindByIdAsync(request.CommentId);

            if(!comment.IsSuccess)
            {
                response.Success = false;
                response.ValidationsErrors = new List<string> { comment.Error };
                return response;
            }
            return new GetByIdCommentQueryResponse
            {
                Success = true,
                Comment = new CommentDto
                {
                    CommentId = comment.Value.CommentId,
                    CommentText = comment.Value.CommentText,
                    CreatedBy = comment.Value.CreatedBy,
                    CreatedDate = comment.Value.CreatedDate,
                    LastModifiedBy = comment.Value.LastModifiedBy,
                    LastModifiedDate = comment.Value.LastModifiedDate,
                    TaskId = comment.Value.TaskId
                   

                }
            };
           
        }
    }
}
