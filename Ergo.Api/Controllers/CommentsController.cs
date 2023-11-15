using Ergo.Application.Features.Comments.Commands.CreateComment;
using Microsoft.AspNetCore.Mvc;

namespace Ergo.Api.Controllers;

public class CommentsController : ApiControllerBase
{
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
}
