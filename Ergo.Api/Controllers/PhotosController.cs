using Ergo.Application.Features.Photos.Commands.AddPhotoToTaskItem;
using Ergo.Application.Features.Photos.Commands.DeletePhoto;
using Ergo.Application.Features.Photos.Queries.GetPhotosForTaskItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ergo.Api.Controllers
{

    public class PhotosController : ApiControllerBase
    {
        [Authorize(Roles = "User")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> AddPhotoToTaskItem([FromForm] AddPhotoToTaskItemCommand command)
        {
            var result = await Mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [Authorize(Roles = "User")]
        [HttpGet("{taskItemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPhotosForTaskItem(Guid taskItemId)
        {
            var result = await Mediator.Send(new GetPhotosForTaskItemQuery { TaskItemId = taskItemId });
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [Authorize(Roles = "User")]
        [HttpDelete("{photoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeletePhoto(Guid photoId)
        {
            var result = await Mediator.Send(new DeletePhotoCommand { PhotoId = photoId });
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
