
using MediatR;

namespace Ergo.Application.Features.Comments.Queries.GetById
{
    public class GetByIdCommentQuery : IRequest<GetByIdCommentQueryResponse>
    {
        public Guid CommentId { get; set; }
    }
}
