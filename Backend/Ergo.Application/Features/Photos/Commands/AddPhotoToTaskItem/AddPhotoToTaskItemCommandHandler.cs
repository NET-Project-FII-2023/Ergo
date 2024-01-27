﻿using Ergo.Application.Persistence;
using Ergo.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ergo.Application.Features.Photos.Commands.AddPhotoToTaskItem
{
    public class AddPhotoToTaskItemCommandHandler : IRequestHandler<AddPhotoToTaskItemCommand, AddPhotoToTaskItemCommandResponse>
    {
        private readonly ITaskItemRepository taskItemRepository;
        private readonly IPhotoRepository photoRepository;
        private readonly IUserRepository userRepository;
        public AddPhotoToTaskItemCommandHandler(ITaskItemRepository taskItemRepository, IPhotoRepository photoRepository, IUserRepository userRepository)
        {
            this.taskItemRepository = taskItemRepository;
            this.photoRepository = photoRepository;
            this.userRepository = userRepository;
        }

        public async Task<AddPhotoToTaskItemCommandResponse> Handle(AddPhotoToTaskItemCommand request, CancellationToken cancellationToken)
        {
            var validator = new AddPhotoToTaskItemCommandValidator();
            var validatorResult = validator.Validate(request);
            if (!validatorResult.IsValid)
            {
                return new AddPhotoToTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }
            var taskItem = await taskItemRepository.FindByIdAsync(request.TaskItemId);
            if (!taskItem.IsSuccess)
            {
                return new AddPhotoToTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { taskItem.Error }
                };
            }
            var image = await ConvertFormFileToByteArray(request.Photo);
            //var base64Image = Convert.ToBase64String(image);
            var photo = Photo.Create(request.TaskItemId, image);
            if (!photo.IsSuccess)
            {
                return new AddPhotoToTaskItemCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { photo.Error }
                };
            }
            await photoRepository.AddAsync(photo.Value);
            return new AddPhotoToTaskItemCommandResponse
            {
                Success = true
            };
        }
        public static async Task<byte[]> ConvertFormFileToByteArray(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

    }
    
}