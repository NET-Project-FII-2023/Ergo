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
        private readonly ITaskItemRepository taskItemRepository;
        public CreateCommentCommandHandler(ICommentRepository commentRepository, ITaskItemRepository taskItemRepository)
        {
            this.commentRepository = commentRepository;
            this.taskItemRepository = taskItemRepository;
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

            var taskItemExists = await taskItemRepository.TaskItemExists(request.TaskId);
            if (!taskItemExists)
            {
                return new CreateCommentCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "TaskItem with the provided ID does not exist." }
                };
            }

            var comment = Comment.Create(request.CreatedBy,request.TaskId,request.CommentText);
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
                    TaskId = request.TaskId
                }
            };
        }
    }
}
