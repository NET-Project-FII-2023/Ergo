using Ergo.Application.Features.TaskItems.Commands.CreateTaskItem;
using Ergo.Application.Features.TaskItems.Commands.DeleteTaskItem;
using Ergo.Application.Features.TaskItems.Commands.UpdateTaskItem;
using Ergo.Application.Features.TaskItems.Queries.GetAll;
using Ergo.Application.Features.TaskItems.Queries.GetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ergo.Api.Controllers;

public class TaskItemsController : ApiControllerBase
{
    [Authorize(Roles = "User")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateTaskItemCommand command)
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
    public async Task<IActionResult> Update(Guid id, UpdateTaskItemCommand command)
    {

        if (id != Guid.Empty)
        {
           command.TaskItemId = id;
        }
        else
        {
            return BadRequest("Input has no id!");
        }
        var result = await Mediator.Send(command);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [Authorize(Roles = "User")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteTaskItem(Guid id)
    {
        var command = new DeleteTaskItemCommand { TaskItemId = id };
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
        var result = await Mediator.Send(new GetAllTaskItemsQuery());
        return Ok(result);
    }

    [Authorize(Roles = "User")]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTaskItemById(Guid id)
    {
        var query = new GetByIdTaskItemQuery { TaskItemId = id };
        var result = await Mediator.Send(query);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

}
