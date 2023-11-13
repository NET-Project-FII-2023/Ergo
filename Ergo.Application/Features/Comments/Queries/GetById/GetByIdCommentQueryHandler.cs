using Ergo.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Features.Comments.Queries.GetById
{
    public class GetByIdCommentQueryHandler : IRequestHandler <GetByIdCommentQuery,CommentDto>
    {
        private readonly ICommentRepository commentRepository;
        
        public GetByIdCommentQueryHandler(ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        public async Task<CommentDto> Handle(GetByIdCommentQuery request, CancellationToken cancellationToken)
        {
            var comment = await commentRepository.FindByIdAsync(request.CommentId);

            if(comment.IsSuccess)
            {
                return new CommentDto
                {
                    CommentId = comment.Value.CommentId,
                    CommentText = comment.Value.CommentText,
                    CreatedDate = comment.Value.CreatedDate,
                    CreatedBy = comment.Value.CreatedBy,
                    LastModifiedDate = comment.Value.LastModifiedDate,
                    LastModifiedBy = comment.Value.LastModifiedBy,
                    Task = comment.Value.Task
                };
            }
            return new CommentDto();
        }
    }
}
