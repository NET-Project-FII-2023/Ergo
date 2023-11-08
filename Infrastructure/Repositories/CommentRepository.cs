using Ergo.Application.Persistence;
using Ergo.Domain.Entities;
using System;

namespace Infrastructure.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ErgoContext context) : base(context)
        {
        }
    }
}
