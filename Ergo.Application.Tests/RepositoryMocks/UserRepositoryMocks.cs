using Ergo.Application.Persistence;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using Moq;
namespace Ergo.Application.Tests.RepositoryMocks
{
    public static class UserRepositoryMocks
    {
        internal readonly static List<User> Users =
        [
            User.Create(new Guid()).Value,
            User.Create(new Guid()).Value
        ];

        public static Mock<IUserRepository> GetUserRepository()
        {
            var mockUserRepository = new Mock<IUserRepository>();

            mockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(Result<IReadOnlyList<User>>.Success(Users));

            mockUserRepository.Setup(repo => repo.FindByIdAsync(It.Is<Guid>(id => id != Users[0].UserId && id != Users[1].UserId)))
                .ReturnsAsync(Result<User>.Failure("Not found"));

            return mockUserRepository;
        }
    }
}
