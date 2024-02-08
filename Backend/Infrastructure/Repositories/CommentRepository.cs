using Ergo.Application.Persistence;
using Ergo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ErgoContext context) : base(context)
        {
        }
        public Task<int> GetNumberOfCommentsByUserIdAsync(string name)
        {
            return context.Comments.CountAsync(x => x.CreatedBy == name);
        }
    }
}
