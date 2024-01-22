using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.Comments.Commands.DeleteComment
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, DeleteCommentCommandResponse>
    {
        private readonly ICommentRepository repository;
        public DeleteCommentCommandHandler(ICommentRepository repository)
        {
            this.repository = repository;
        }

        public async Task<DeleteCommentCommandResponse> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            DeleteCommentCommandResponse response = new();
            var validator = new DeleteCommentCommandValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.ValidationsErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return response;
            }
            var commentToDelete = await repository.FindByIdAsync(request.CommentId);

            if (commentToDelete == null)
            {
                response.Success = false;
                response.ValidationsErrors = new List<string> { "Comment not found" };
                return response;
            }
            var result = await repository.DeleteAsync(request.CommentId);
            if (!result.IsSuccess)
            {
                response.Success = false;
                response.ValidationsErrors = new List<string> { result.Error };
                return response;
            }
            return new DeleteCommentCommandResponse
            {
                Success = true
            };

        }
    }
}
