using Amazon.Runtime;
using Ergo.Api.Controllers;
using Ergo.Api.Models;
using Ergo.Api.Models.AwsS3;
using Ergo.Api.Services;
using Ergo.Application.Features.Photos.Commands.AddPhotoToTaskItem;
using Ergo.Application.Features.Photos.Commands.DeletePhoto;
using Ergo.Application.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ergo.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CloudController : ApiControllerBase
    {
        private readonly IStorageService storageService;
        private readonly IConfiguration configuration;
        private readonly IPhotoRepository photoRepository;

        public CloudController(IStorageService storageService, IConfiguration configuration, IPhotoRepository photoRepository)
        {
            this.storageService = storageService;
            this.configuration = configuration;
            this.photoRepository = photoRepository;
        }

        [HttpPost]
        [Route("upload-photo")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> UploadFile(AddPhotoDto addPhoto)
        {
            await using var memoryStr = new MemoryStream();
            await addPhoto.File.CopyToAsync(memoryStr);

            var fileExt = Path.GetExtension(addPhoto.File.FileName);
            var objName = $"{Guid.NewGuid()}{fileExt}";
            var s3Object = new S3Object()
            {
                BucketName = "ergo-project",
                InputStream = memoryStr,
                Name = objName
            };
            var cred = new AWSCredential()
            {
                AwsKey = configuration["AwsConfiguration:AWSAccessKey"],
                AwsSecretKey = configuration["AwsConfiguration:AWSSecretKey"]
            };
            var result = await storageService.UploadFileAsync(s3Object, cred);
            var command = new AddPhotoToTaskItemCommand()
            {
                CloudURL = objName,
                TaskItemId = addPhoto.TaskItemId
            };
            var photoResult = await Mediator.Send(command);
            if (!photoResult.Success)
            {
                return BadRequest(photoResult);
            }
            return Ok();
        }
        [HttpDelete]
        [Route("delete-photo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteFile(DeletePhotoCommand command)
        {
            var photo = await photoRepository.FindByIdAsync(command.PhotoId);
            if (!photo.IsSuccess)
            {
                return BadRequest(photo);
            }
            var cred = new AWSCredential()
            {
                AwsKey = configuration["AwsConfiguration:AWSAccessKey"],
                AwsSecretKey = configuration["AwsConfiguration:AWSSecretKey"]
            };
            var result = await storageService.DeleteFileAsync(photo.Value.CloudURL, cred);
            if (!result)
            {
                return BadRequest(result);
            }
            var deletePhotoResult = await Mediator.Send(command);
            if (!deletePhotoResult.Success)
            {
                return BadRequest(deletePhotoResult);
            }
            return Ok();

        }

    }

}
