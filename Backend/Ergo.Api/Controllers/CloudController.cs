using Amazon.Runtime;
using Ergo.Api.Controllers;
using Ergo.Api.Models;
using Ergo.Api.Models.AwsS3;
using Ergo.Api.Services;
using Ergo.Application.Features.Photos.Commands.AddPhotoToTaskItem;
using Ergo.Application.Features.Photos.Commands.DeletePhoto;
using Ergo.Application.Features.UserPhotos.Commands.AddUserPhoto;
using Ergo.Application.Features.UserPhotos.Commands.UpdateTaskPhoto;
using Ergo.Application.Persistence;
using Ergo.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ergo.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CloudController : ApiControllerBase
    {
        private readonly IStorageService storageService;
        private readonly IPhotoRepository photoRepository;
        private readonly IUserPhotoRepository userPhotoRepository;
        private string AwsKeyEnv { get; set; }
        private string AwsSecretKeyEnv { get; set; }

        public CloudController(IStorageService storageService, IPhotoRepository photoRepository, IUserPhotoRepository userPhotoRepository)
        {

            this.storageService = storageService;
            this.photoRepository = photoRepository;
            AwsKeyEnv = DotNetEnv.Env.GetString("AWSAccessKey");
            AwsSecretKeyEnv = DotNetEnv.Env.GetString("AWSSecretKey");
            this.userPhotoRepository = userPhotoRepository;
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [Route("upload-task-photo")]
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
                AwsKey = AwsKeyEnv,
                AwsSecretKey = AwsSecretKeyEnv
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
                await storageService.DeleteFileAsync(objName, cred);
                return BadRequest(photoResult);
            }
            return Ok();
        }

        [Authorize(Roles = "User")]
        [HttpDelete]
        [Route("delete-task-photo")]
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
                AwsKey = AwsKeyEnv,
                AwsSecretKey = AwsSecretKeyEnv
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

        [Authorize(Roles = "User")]
        [HttpPost]
        [Route("upload-user-photo")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> AddUserPhoto(AddUserPhotoDto addUserPhotoDto)
        {
            await using var memoryStr = new MemoryStream();
            await addUserPhotoDto.File.CopyToAsync(memoryStr);
            var fileExt = Path.GetExtension(addUserPhotoDto.File.FileName);
            var objName = $"{Guid.NewGuid()}{fileExt}";
            var s3Object = new S3Object()
            {
                BucketName = "ergo-project",
                InputStream = memoryStr,
                Name = objName
            };
            var cred = new AWSCredential()
            {
                AwsKey = AwsKeyEnv,
                AwsSecretKey = AwsSecretKeyEnv
            };
            var result = await storageService.UploadFileAsync(s3Object, cred);
            var command = new AddUserPhotoCommand()
            {
                UserId = addUserPhotoDto.UserId,
                PhotoUrl = objName
            };
            var userPhotoResult = await Mediator.Send(command);
            if (!userPhotoResult.Success)
            {
                await storageService.DeleteFileAsync(objName, cred);
                return BadRequest(userPhotoResult);
            }
            return Ok(userPhotoResult);

        }

        [Authorize(Roles = "User")]
        [HttpPut]
        [Route("update-user-photo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUserPhoto(UpdateUserPhotoDto updateUserPhotoDto)
        {
            await using var memoryStr = new MemoryStream();
            await updateUserPhotoDto.File.CopyToAsync(memoryStr);
            var fileExt = Path.GetExtension(updateUserPhotoDto.File.FileName);
            var objName = $"{Guid.NewGuid()}{fileExt}";
            var s3Object = new S3Object()
            {
                BucketName = "ergo-project",
                InputStream = memoryStr,
                Name = objName
            };
            var cred = new AWSCredential()
            {
                AwsKey = AwsKeyEnv,
                AwsSecretKey = AwsSecretKeyEnv
            };

            var result = await storageService.UploadFileAsync(s3Object, cred);
            var command = new UpdateUserPhotoCommand()
            {
                UserPhotoId = updateUserPhotoDto.UserPhotoId,
                PhotoUrl = objName
            };
            var userPhotoResult = await Mediator.Send(command);
            if (!userPhotoResult.Success)
            {
                await storageService.DeleteFileAsync(objName, cred);
                return BadRequest(userPhotoResult);
            }
            var deleteOldPhoto = await storageService.DeleteFileAsync(updateUserPhotoDto.CloudUrl, cred);
            if (!deleteOldPhoto)
            {
                return BadRequest(deleteOldPhoto);
            }
            return Ok(userPhotoResult);
        }
    }
}
