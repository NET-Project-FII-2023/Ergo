using Ergo.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Features.Comments.Commands.CreateComment
{
    public class CreateCommentCommandResponse : BaseResponse
    {
        public CreateCommentCommandResponse():base()
        {
        }
        public CreateCommentDto Comment { get; set; }
    }
}
