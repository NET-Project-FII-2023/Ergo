using Ergo.Application.Features.InboxItems.Commands.CreateInboxItem;
using Ergo.Application.Features.InboxItems.Commands.UpdateInboxItemIsRead;
using Ergo.Application.Features.InboxItems.Queries.GetByUserId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ergo.Api.Controllers
{

    public class InboxItemController : ApiControllerBase
    {
        [Authorize(Roles = "User")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(CreateInboxItemCommand command)
        {
            var result = await Mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [Authorize(Roles = "User")]
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var result = await Mediator.Send(new GetByUserIdQuery { UserId = userId });
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [Authorize(Roles = "User")]
        [HttpPut("{inboxItemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateIsRead(Guid inboxItemId)
        {
            var result = await Mediator.Send(new UpdateInboxItemIsReadCommand { InboxItemId = inboxItemId });
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
