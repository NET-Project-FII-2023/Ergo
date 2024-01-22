using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ergo.Application.Features.Photos.Commands.AddPhotoToTaskItem
{
    public class AddPhotoToTaskItemCommand : IRequest<AddPhotoToTaskItemCommandResponse>
    {
        public Guid TaskItemId { get; set; }
        public IFormFile Photo { get; set; }
    }
}
