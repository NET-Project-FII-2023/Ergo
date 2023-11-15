using Ergo.Application.Persistence;
using Ergo.Domain.Entities;
using MediatR;

namespace Ergo.Application.Features.Comments.Commands.UpdateComment
{
    public class UpdateCommentCommandHandler: IRequestHandler<UpdateCommentCommand,UpdateCommentCommandResponse>
    {
        private readonly ICommentRepository commentRepository;

        public UpdateCommentCommandHandler(ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        public async Task<UpdateCommentCommandResponse> Handle (UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await commentRepository.FindByIdAsync(request.CommentId);
            if (!comment.IsSuccess)
            {
                return new UpdateCommentCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Comment with id this does not exists" }
                };
            }
            request.CreatedDate = comment.Value.CreatedDate;
            request.CreatedBy ??= comment.Value.CreatedBy;
            request.LastModifiedDate = DateTime.Now;
            request.LastModifiedBy = comment.Value.LastModifiedBy;
            request.CommentText ??= comment.Value.CommentText;
            request.Task ??= comment.Value.Task;

            var validator = new UpdateCommentCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if(!validationResult.IsValid)
            {
                return new UpdateCommentCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList()
                };
            }
            
            comment.Value.UpdateData(request.CreatedBy, request.CreatedDate, request.LastModifiedBy, request.LastModifiedDate, request.CommentText, request.Task);
            
            var result = await commentRepository.UpdateAsync(comment.Value);
            return new UpdateCommentCommandResponse
            {
                Success = true,
                Comment = new UpdateCommentDto
                {
                    CreatedBy = result.Value.CreatedBy,
                    CreatedDate = result.Value.CreatedDate,
                    LastModifiedBy = result.Value.LastModifiedBy,
                    LastModifiedDate = result.Value.LastModifiedDate,
                    CommentText = result.Value.CommentText,
                    Task = result.Value.Task
                }
            };
        }

    }
}
