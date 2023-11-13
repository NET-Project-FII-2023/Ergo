using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Features.Comments.Queries.GetAll
{
    public class GetAllCommentsResponse
    {
        public List<CommentDto> Comments { get; set; }
    }
}
