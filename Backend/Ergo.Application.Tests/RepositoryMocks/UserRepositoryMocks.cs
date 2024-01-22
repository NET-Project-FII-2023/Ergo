using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using NSubstitute;

namespace Ergo.Application.Tests.RepositoryMocks
{
    public static class UserRepositoryMocks
    {
        internal readonly static List<User> Users =
        [
            User.Create(new Guid()).Value,
            User.Create(new Guid()).Value
        ];

        public static IUserRepository GetUserRepository()
        {
            var mockUserRepository = Substitute.For<IUserRepository>();

            mockUserRepository.GetAllAsync().Returns(Result<IReadOnlyList<User>>.Success(Users));

            mockUserRepository.FindByIdAsync(Arg.Is<Guid>(id => id != Users[0].UserId && id != Users[1].UserId))
                .Returns(Result<User>.Failure("Not found"));

            return mockUserRepository;
        }
    }
}
