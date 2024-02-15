using Ergo.Application.Features.Users.Queries;
using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Identity.Models;
using NSubstitute;

namespace Ergo.Application.Tests.RepositoryMocks
{
    public static class UserManagerRepositoryMocks
    {
        internal readonly static List<UserDto> Users =
        [
            new UserDto { UserId = "1", Username = "User 1"},
            new UserDto { UserId = "2", Username = "User 2" }
            
        ];
        //public static IUserManager GetUserManagerRepository()
        //{
        //    var mockUserManager = Substitute.For<IUserManager>();
        //    mockUserManager.GetAllAsync().Returns(Result<IReadOnlyList<UserDto>>.Success(Users));

            
        //}
       
    }
}
