using Ergo.Application.Features.Comments.Commands.CreateComment;
using Ergo.Application.Features.Comments.Commands.DeleteComment;
using Ergo.Application.Features.Comments.Commands.UpdateComment;
using Ergo.Application.Features.Comments.Queries.GetAll;
using Ergo.Application.Features.Comments.Queries.GetById;
using Ergo.Application.Features.Comments.Queries.GetByTaskId;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Ergo.Api.Controllers
{
    public class CommentsController : ApiControllerBase
    {
        [Authorize(Roles = "User")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(CreateCommentCommand command)
        {
            var result = await Mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, UpdateCommentCommand command)
        {
            if (id != command.CommentId)
            {
                return BadRequest("The ids must be the same!");
            }
            var result = await Mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await Mediator.Send(new GetAllCommentsQuery());
            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id,DeleteCommentCommand command)
        {
            if (id != command.CommentId)
            {
                return BadRequest("The ids must be the same!");
            }
            var result = await Mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCommentById(Guid id)
        {
            var query = new GetByIdCommentQuery { CommentId = id };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet("ByTaskId/{taskId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCommentByTaskId(Guid taskId)
        {
			var query = new GetCommentByTaskIdQuery { TaskId = taskId };
			var result = await Mediator.Send(query);
			if(!result.Success)
            {
				return BadRequest(result);
			}
            return Ok(result);
		}
    }
}
