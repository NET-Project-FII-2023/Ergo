using Ergo.Application.Persistence;
using MediatR;

namespace Ergo.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand,DeleteUserCommandResponse>
    {
        private readonly IUserRepository repository;
        public DeleteUserCommandHandler(IUserRepository repository)
        {
            this.repository = repository;
        }

        public async Task<DeleteUserCommandResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
           DeleteUserCommandResponse response = new();
           var validator = new DeleteUserCommandValidator();
           var validationResult = await validator.ValidateAsync(request);
            if(!validationResult.IsValid)
            {
                response.Success = false;
                response.ValidationsErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return response;
            }
           var userToDelete = await repository.FindByIdAsync(request.UserId);
            
            if (userToDelete == null)
           {
               response.Success = false;
               response.ValidationsErrors = new List<string> { "User not found" };
               return response;
           }
            await Console.Out.WriteLineAsync("aici sunt");
           var result = await repository.DeleteAsync(request.UserId);
           if(!result.IsSuccess)
            {
                response.Success = false;
                response.ValidationsErrors = new List<string> { result.Error };
                return response;
            }
           return new DeleteUserCommandResponse
           {
                Success = true
            };

        }
    }
}
