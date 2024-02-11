using Ergo.Application.Features.Users;
using Ergo.Application.Features.Users.Queries;
using Ergo.Application.Models;
using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Ergo.Identity.Services
{
    public class ApplicationUserManager : IUserManager
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUserPhotoRepository userPhotoRepository;

        public ApplicationUserManager(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IUserPhotoRepository userPhotoRepository)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userPhotoRepository = userPhotoRepository;
        }

        public async Task<Result<UserDto>> FindByIdAsync(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if(user == null)
            {
                return Result<UserDto>.Failure($"User with id {userId} not found");
            }

            var userDtos = MapToUserDto(user);
            var roles = await userManager.GetRolesAsync(user);
            userDtos.Roles = roles.ToList();

            var userPhoto = await userPhotoRepository.GetUserPhotoByUserIdAsync(user.Id);
            if (userPhoto.IsSuccess)
            {
                userDtos.UserPhoto = new UserCloudPhotoDto
                {
                    UserPhotoId = userPhoto.Value.UserPhotoId,
                    PhotoUrl = userPhoto.Value.PhotoUrl
                };
            }

            return Result<UserDto>.Success(userDtos);
        }

        public async Task<Result<UserDto>> FindByEmailAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<UserDto>.Failure($"User with email {email} not found");
            var userDtos = MapToUserDto(user);
            var roles = await userManager.GetRolesAsync(user);
            userDtos.Roles = roles.ToList();
            return Result<UserDto>.Success(userDtos);
        }
        public async Task<Result<UserDto>> FindByUsernameAsync(string username)
        {

            var user = await userManager.FindByNameAsync(username);
            if (user == null)
                return Result<UserDto>.Failure($"User with username {username} not found");
            var userDtos = MapToUserDto(user);
            var roles = await userManager.GetRolesAsync(user);
            userDtos.Roles = roles.ToList();
            return Result<UserDto>.Success(userDtos);
        }



        public async Task<Result<List<UserDto>>> GetAllAsync()
        {
            var users = userManager.Users.ToList();
            var userDtos = users.Select(u => MapToUserDto(u)).ToList();

            foreach (var userDto in userDtos)
            {
                var appUser = await userManager.FindByIdAsync(userDto.UserId.ToString());
                if (appUser != null)
                {
                    var roles = await userManager.GetRolesAsync(appUser);
                    userDto.Roles = roles.ToList();
                }
            }

            return Result<List<UserDto>>.Success(userDtos);
        }


        public async Task<Result<UserDto>> DeleteAsync(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return Result<UserDto>.Failure($"User with id {userId} not found");

            var deleteResult = await userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)
                return Result<UserDto>.Failure($"User with id {userId} not deleted");

            return Result<UserDto>.Success(MapToUserDto(user));
        }


        public async Task<Result<UserDto>> UpdateAsync(UserDto userDto)
        {
            var userToUpdate = await userManager.FindByIdAsync(userDto.UserId.ToString());
            if (userToUpdate == null)
                return Result<UserDto>.Failure($"User with id {userDto.UserId} not found");

            UpdateUserProperties(userToUpdate, userDto);

            var updateResult = await userManager.UpdateAsync(userToUpdate);
            return updateResult.Succeeded ? Result<UserDto>.Success(MapToUserDto(userToUpdate)) : Result<UserDto>.Failure($"User with id {userDto.UserId} not updated");
        }

        public async Task<Result<UserDto>> UpdateRoleAsync(UserDto userDto, string role)
        {
            var userToUpdate = await userManager.FindByIdAsync(userDto.UserId.ToString());
            if (userToUpdate == null)
                return Result<UserDto>.Failure($"User with id {userDto.UserId} not found");
            
            if (role != "Admin" && role != "User")
                return Result<UserDto>.Failure($"Role {role} not found");

            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

            if (await userManager.IsInRoleAsync(userToUpdate, role))
                return Result<UserDto>.Failure($"User with id {userDto.UserId} already has role {role}");

            var addToRoleResult = await userManager.AddToRoleAsync(userToUpdate, role);
            return addToRoleResult.Succeeded ? Result<UserDto>.Success(MapToUserDto(userToUpdate)) : Result<UserDto>.Failure($"User with id {userDto.UserId} not updated");
        }
        private void UpdateUserProperties(ApplicationUser user, UserDto userDto)
        {
            user.Name = userDto.Name;
            user.UserName = userDto.Username;
            user.Email = userDto.Email;
            user.Bio = userDto.Bio;
            user.Mobile = userDto.Mobile;
            user.Company = userDto.Company;
            user.Location = userDto.Location;
            user.Facebook = userDto.Social?.Facebook;
            user.Instagram = userDto.Social?.Instagram;
            user.GitHub = userDto.Social?.GitHub;
            user.LinkedIn = userDto.Social?.LinkedIn;
            user.TwitterX = userDto.Social?.TwitterX;
        }
        private UserDto MapToUserDto(ApplicationUser user)
        {
            return new UserDto
            {
                UserId = user.Id,
                Name = user.Name,
                Username = user.UserName,
                Email = user.Email,
                Bio = user.Bio,
                Mobile = user.Mobile,
                Company = user.Company,
                Location = user.Location,
                Social = new Social
                {
                    Facebook = user.Facebook,
                    Instagram = user.Instagram,
                    GitHub = user.GitHub,
                    LinkedIn = user.LinkedIn,
                    TwitterX = user.TwitterX
                }
            };
        }


    }
}
