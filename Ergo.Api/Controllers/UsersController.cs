using Ergo.Application.Features.Users.Commands.CreateUser;
using Ergo.Application.Features.Users.Commands.DeleteUser;
using Ergo.Application.Features.Users.Commands.UpdateUser;
using Ergo.Application.Features.Users.Queries.GetAll;
using Ergo.Application.Features.Users.Queries.GetById;
using Microsoft.AspNetCore.Mvc;

namespace Ergo.Api.Controllers;

public class UsersController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateUserCommand command)
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
    public async Task<IActionResult> Update(Guid id,UpdateUserCommand command)
    {
        if (id != command.UserId)
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
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllUsersQuery());
        return Ok(result);
    }
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteUserCommand { UserId = id };
        var result = await Mediator.Send(command);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var query = new GetByIdUserQuery(id);
        var result = await Mediator.Send(query);

        return Ok(result);
    }
}
