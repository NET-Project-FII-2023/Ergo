using Ergo.Application.Features.Projects.Commands.CreateProject;
using Ergo.Application.Features.Projects.Commands.UpdateProject;
using Ergo.Application.Features.Projects.Commands.DeleteProject;
using Ergo.Application.Features.Projects.Queries.GetAll;
using Ergo.Application.Features.Projects.Queries.GetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ergo.Application.Features.Projects.Commands.AssignUserToProject;
using Ergo.Application.Features.Projects.Queries.GetProjectsByUserId;
using Ergo.Application.Features.Projects.Commands.DeleteUserFromProject;

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
    [Authorize(Roles = "User")]
    [HttpPost]
    [Route("AssignUserToProject")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AssignUserToProject(AssignUserToProjectCommand command)
    {
        var result = await Mediator.Send(command);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
    [Authorize(Roles ="User")]
    [HttpDelete]
    [Route("DeleteUserFromProject")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> DeleteUserFromProject(DeleteUserFromProjectCommand command)
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
    [Route("GetProjectsByUserId/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProjectsByUserId(string userId)
    {
        var query = new GetProjectsByUserIdQuery { UserId = userId};
        var result = await Mediator.Send(query);
        return Ok(result);
    }
}
