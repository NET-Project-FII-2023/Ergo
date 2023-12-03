using Ergo.Application.Features.Projects.Commands.CreateProject;
using Ergo.Application.Features.Projects.Commands.UpdateProject;
using Ergo.Application.Features.Projects.Commands.DeleteProject;
using Ergo.Application.Features.Projects.Queries.GetAll;
using Ergo.Application.Features.Projects.Queries.GetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ergo.Api.Controllers;

public class ProjectsController : ApiControllerBase
{
    [Authorize(Roles = "User")]
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

    [Authorize(Roles = "User")]
    [HttpPut ("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(UpdateProjectCommand command)
    {
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
        var result = await Mediator.Send(new GetAllProjectsQuery());
        return Ok(result);
    }

    [Authorize(Roles = "User")]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetByIdProjectQuery (id);
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [Authorize(Roles = "User")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteProjectCommand { ProjectId = id };
        var result = await Mediator.Send(command);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
}
