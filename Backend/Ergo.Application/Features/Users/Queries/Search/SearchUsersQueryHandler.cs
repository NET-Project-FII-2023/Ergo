using Ergo.Application.Persistence;
using MediatR;
namespace Ergo.Application.Features.Users.Queries.Search
{
    public class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, SearchUsersQueryResponse>
    {
        private readonly IUserManager userManager;
        public SearchUsersQueryHandler(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public async Task<SearchUsersQueryResponse> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
        {
            var allUsers = await userManager.GetAllAsync();
            if (!allUsers.IsSuccess)
            {
                return new SearchUsersQueryResponse { Success = false, Message = allUsers.Error };
            }

            var users = allUsers.Value.Where(u =>
                    (!string.IsNullOrWhiteSpace(u.Username) && u.Username.ToLower().Contains(request.SearchValue.ToLower())) ||
                    (!string.IsNullOrWhiteSpace(u.Name) && u.Name.ToLower().Contains(request.SearchValue.ToLower()))
                ).ToArray();

            return new SearchUsersQueryResponse
            {
                Success = true,
                Users = users.Select(u => new UserSearchDto
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    Name = u.Name,
                    Email = u.Email
                }).Take(25).ToArray()
            };
        }
    }
}
