using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ergo.Application.Features.Photos.Commands.AddPhotoToTaskItem
{
    public class AddPhotoToTaskItemCommand : IRequest<AddPhotoToTaskItemCommandResponse>
    {
        public IFormFile File { get; set; }
        public Guid TaskItemId { get; set; }
        public string CloudURL { get; set; }
    }
}
