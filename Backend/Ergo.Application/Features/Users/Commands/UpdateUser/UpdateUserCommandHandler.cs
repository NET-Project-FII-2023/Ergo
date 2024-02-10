using Ergo.Application.Features.Users.Queries;
using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserCommandResponse>
    {
        private readonly IUserManager userRepository;
        private readonly IUserPhotoRepository userPhotoRepository;

        public UpdateUserCommandHandler(IUserManager userRepository, IUserPhotoRepository userPhotoRepository)
        {
            this.userRepository = userRepository;
            this.userPhotoRepository = userPhotoRepository;
        }

        public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateUserCommandResponse();

            var user = await userRepository.FindByIdAsync(request.Id);
            if(!user.IsSuccess)
            {
                return new UpdateUserCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "User with id this does not exists" }
                };
            }

            request.Name ??= user.Value.Name;
            request.Username ??= user.Value.Username;
            request.Email ??= user.Value.Email;
            request.Bio ??= user.Value.Bio;
            request.Mobile ??= user.Value.Mobile;
            request.Company ??= user.Value.Company;
            request.Location ??= user.Value.Location;
            request.Social ??= user.Value.Social;

            var validator = new UpdateUserCommandValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return new UpdateUserCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList()
                };
            }

            var userByEmail = await userRepository.FindByEmailAsync(request.Email);
            if (userByEmail.IsSuccess && userByEmail.Value.UserId != user.Value.UserId)
            {
                Console.WriteLine("Email" + request.Email);

                return new UpdateUserCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Email already exists" }
                };
            }

            var userByUsername = await userRepository.FindByUsernameAsync(request.Username);
            if (userByUsername.IsSuccess && userByUsername.Value.UserId != user.Value.UserId)
            {
                return new UpdateUserCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Username already exists" }
                };
            }

            var userPhoto = await userPhotoRepository.GetUserPhotoByUserIdAsync(request.Id.ToString());
            UserDto userDto = new()
            {
                UserId = user.Value.UserId,
                Name = request.Name,
                Username = request.Username,
                Email = request.Email,
                Bio = request.Bio,
                Mobile = request.Mobile,
                Company = request.Company,
                Location = request.Location,
                Social = request.Social
            };

            var result = await userRepository.UpdateAsync(userDto);
            if (!result.IsSuccess)
            {
                return new UpdateUserCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { result.Error }
                };
            }

            return new UpdateUserCommandResponse
            {
                Success = true,
                User = new UpdateUserDto
                {
                    Name = result.Value.Name,
                    Username = result.Value.Username,
                    UserPhoto = userPhoto.IsSuccess ? new UserCloudPhotoDto
                    {
                        UserPhotoId = userPhoto.Value.UserPhotoId,
                        PhotoUrl = userPhoto.Value.PhotoUrl
                    } : null,
                    Email = result.Value.Email,
                    Bio = result.Value.Bio,
                    Mobile = result.Value.Mobile,
                    Company = result.Value.Company,
                    Location = result.Value.Location,
                    Social = result.Value.Social
                }
            };
        }
    }
}
