using Ergo.Application.Features.ML.Query.GetTaskPrediction;
using Ergo.Domain.Entities.ML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ergo.Api.Controllers
{

    public class MLController : ApiControllerBase
    {
        [Authorize(Roles = "User")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPrediction(TaskDataDto taskData)
        {
            var result = await Mediator.Send(new TaskPredictionQuery { TaskData = taskData });
            return Ok(result);
        }
    }
}
