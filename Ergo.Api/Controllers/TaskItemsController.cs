using Ergo.Application.Features.TaskItems.Commands.CreateTaskItem;
using Ergo.Application.Features.TaskItems.Commands.DeleteTaskItem;
using Ergo.Application.Features.TaskItems.Commands.UpdateTaskItem;
using Ergo.Application.Features.TaskItems.Queries.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace Ergo.Api.Controllers;

public class TaskItemsController : ApiControllerBase
{
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


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllTaskItemsQuery());
        return Ok(result);
    }
}
