using Ergo.Application.Features.Projects.Commands.CreateProject;
using Microsoft.AspNetCore.Mvc;

namespace Ergo.Api.Controllers;

public class ProjectsController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateProjectCommand command)
    {
        var result = await Mediator.Send(command);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
}
