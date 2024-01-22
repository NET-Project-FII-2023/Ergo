using MediatR;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Ergo.Application.Features.TaskItems.Commands.AddManualTimeTaskItem
{
    public class AddManualTimeTaskItemCommand : IRequest<AddManualTimeTaskItemCommandResponse>
    {
        public Guid TaskItemId { get; set; }
        public Guid UserId { get; set; }

        [SwaggerSchema(Format = "time-span")]
        [DataType(DataType.Duration)]
        public TimeSpan TimeSpent { get; set; }
    }
}
