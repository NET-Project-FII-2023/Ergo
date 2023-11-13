using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Ergo.Application.Features.Comments.Queries.GetById
{
    public record GetByIdCommentQuery(Guid CommentId) : IRequest<CommentDto>;
}
