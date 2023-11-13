using Ergo.Application.Persistence;
using Ergo.Domain.Entities;
using Ergo.Domain.Entities.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Features.Comments.Commands.CreateComment
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CreateCommentCommandResponse>
    {
        private readonly ICommentRepository commentRepository;
        public CreateCommentCommandHandler(ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        public async Task<CreateCommentCommandResponse> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateCommentCommandValidator();
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validatorResult.IsValid) 
            {
                return new CreateCommentCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }
            var comment = Comment.Create(request.CreatedBy,request.Task,request.CommentText);
            if (!comment.IsSuccess)
            {
                return new CreateCommentCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { comment.Error }
                };
            }
            await commentRepository.AddAsync(comment.Value);
            return new CreateCommentCommandResponse
            {
                Success = true,
                Comment = new CreateCommentDto
                {
                    
                    CreatedBy = request.CreatedBy,
                    CreatedDate = request.CreatedDate,
                    LastModifiedBy = request.LastModifiedBy,
                    LastModifiedDate = request.LastModifiedDate,
                    CommentText = request.CommentText,
                    Task = request.Task
                }
            };
        }
    }
}
