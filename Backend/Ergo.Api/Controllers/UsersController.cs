using Ergo.Application.Features.UserPhotos.Queries.GetUserPhoto;
using Ergo.Application.Features.Users.Commands.DeleteUser;
using Ergo.Application.Features.Users.Commands.UpdateRole;
using Ergo.Application.Features.Users.Commands.UpdateUser;
using Ergo.Application.Features.Users.Queries.GetAll;
using Ergo.Application.Features.Users.Queries.GetByEmail;
using Ergo.Application.Features.Users.Queries.GetById;
using Ergo.Application.Features.Users.Queries.GetByProjectId;
using Ergo.Application.Features.Users.Queries.GetUserStats;
using Ergo.Application.Features.Users.Queries.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ergo.Api.Controllers;

public class UsersController : ApiControllerBase
{
    //[Authorize(Roles = "User")]
    //[HttpPost]
    //[ProducesResponseType(StatusCodes.Status201Created)]
    //public async Task<IActionResult> Create(CreateUserCommand command)
    //{
    //    var result = await Mediator.Send(command);
    //    if (!result.Success)
    //    {
    //        return BadRequest(result);
    //    }
    //    return Ok(result);
    //}

    //[Authorize(Roles = "User")]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid id, UpdateUserCommand command)
    {
        if (id != command.Id)
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
        var result = await Mediator.Send(new GetAllUsersQuery());
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "User")]
    [HttpGet("ById/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserById(string id)
    {
        var query = new GetByIdUserQuery { UserId = id };
        var result = await Mediator.Send(query);
        if(!result.Success)
        {
            if(result.Message == $"User with id {id} not found")
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }
        return Ok(result);
    }

    [Authorize(Roles = "User")]
    [HttpGet("ByEmail/{email}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var query = new GetByEmailUserQuery { Email = email };
        var result = await Mediator.Send(query);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [Authorize(Roles ="Admin")]
    [HttpPut("/role/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateRole(Guid id, UpdateUserRoleCommand command)
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

    [Authorize(Roles = "User")]
    [HttpGet("ByProjectId/{projectId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsersByProjectId(string projectId)
    {
        var query = new GetUsersByProjectIdQuery { ProjectId = projectId };
        var result = await Mediator.Send(query);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [Authorize(Roles = "User")]
    [HttpGet("photo/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPhotoByUserId(string id)
    {
        var query = new GetUserPhotoQuery() { UserId = id };
        var result = await Mediator.Send(query);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
    [Authorize(Roles = "User")]
    [HttpGet("stats/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserStats(string userId)
    {
        var query = new GetUserStatsQuery() { UserId = userId };
        var result = await Mediator.Send(query);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [Authorize(Roles = "User")]
    [HttpGet("search/{searchValue}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchUsers(string searchValue)
    {
        var query = new SearchUsersQuery { SearchValue = searchValue };
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    //[HttpPost]
    //[Route("reset-code")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //public async Task<IActionResult> ResetCode(ResetCodeCommand command)
    //{
    //    var result = await Mediator.Send(command);
    //    if (!result.Success)
    //    {
    //        return BadRequest(result);
    //    }
    //    return Ok(result);
    //}
}
