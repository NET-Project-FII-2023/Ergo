﻿using Ergo.Application.Features.Users.Queries;
using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Ergo.Identity.Services
{
    public class ApplicationUserManager : IUserManager
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ApplicationUserManager(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }


        public async Task<Result<UserDto>> FindByIdAsync(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return Result<UserDto>.Failure($"User with id {userId} not found");
            var userDto = new UserDto
            {
                UserId = user.Id,
                Name = user.Name,
                Username = user.UserName,
                Email = user.Email,
                Password = user.PasswordHash,
            };
            return Result<UserDto>.Success(userDto);
            
        }

        public Task<Result<List<UserDto>>> GetAllAsync()
        {
            var users = userManager.Users.ToList();
            var userDtos = users.Select(u => new UserDto
            {
                UserId = u.Id,
                Name = u.Name,
                Username = u.UserName,
                Email = u.Email,
                Password = u.PasswordHash,
            }).ToList();
            return Task.FromResult(Result<List<UserDto>>.Success(userDtos));
        }

        public Task<Result<UserDto>> DeleteAsync(Guid userId)
        {
            var user = userManager.FindByIdAsync(userId.ToString()).Result;
            if (user == null)
                return Task.FromResult(Result<UserDto>.Failure($"User with id {userId} not found"));
            var result = userManager.DeleteAsync(user).Result;
            if (!result.Succeeded)
                return Task.FromResult(Result<UserDto>.Failure($"User with id {userId} not deleted"));
            var userDto = new UserDto
            {
                UserId = user.Id,
                Name = user.Name,
                Username = user.UserName,
                Email = user.Email,
                Password = user.PasswordHash,
            };
            return Task.FromResult(Result<UserDto>.Success(userDto));
        }

        public Task<Result<UserDto>> FindByEmailAsync(string email)
        {
            var user = userManager.FindByEmailAsync(email).Result;
            if (user == null)
                return Task.FromResult(Result<UserDto>.Failure($"User with email {email} not found"));
            var userDto = new UserDto
            {
                UserId = user.Id,
                Name = user.Name,
                Username = user.UserName,
                Email = user.Email,
                Password = user.PasswordHash,
            };
            return Task.FromResult(Result<UserDto>.Success(userDto));
        }

        public async Task<Result<UserDto>> UpdateAsync(UserDto user)
        {
            var userToUpdate = await userManager.FindByIdAsync(user.UserId.ToString());
            if (userToUpdate == null)
                return Result<UserDto>.Failure($"User with id {user.UserId} not found");
            userToUpdate.Name = user.Name;
            await Console.Out.WriteLineAsync(userToUpdate.Name);
            userToUpdate.UserName = user.Username;
            userToUpdate.NormalizedUserName = user.Username.ToUpper();
            userToUpdate.Email = user.Email;
            userToUpdate.NormalizedEmail = user.Email.ToUpper();
            userToUpdate.PasswordHash = user.Password;
            var result = await userManager.UpdateAsync(userToUpdate);
            if (!result.Succeeded)
                return Result<UserDto>.Failure($"User with id {user.UserId} not updated");
            var userDto = new UserDto
            {
                UserId = userToUpdate.Id,
                Name = userToUpdate.Name,
                Username = userToUpdate.UserName,
                Email = userToUpdate.Email,
                Password = userToUpdate.PasswordHash,
            };
            return Result<UserDto>.Success(userDto);
            
        }
    }
}
