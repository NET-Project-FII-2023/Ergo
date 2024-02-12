
using Ergo.Application.Features.Statistics.Queries.GetTasksDueThisWeek;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ergo.Api.Controllers;

public class StatisticsController : ApiControllerBase
{
    [Authorize(Roles = "User")]
    [HttpGet("GetTasksDueThisWeek/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTasksDueThisWeek(Guid userId)
    {
        var query = new GetTasksDueThisWeekQuery { UserId = userId };
        var result = await Mediator.Send(query);
        return Ok(result);
    }   
}