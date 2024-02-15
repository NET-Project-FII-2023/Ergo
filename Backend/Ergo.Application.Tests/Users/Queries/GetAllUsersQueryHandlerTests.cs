using Ergo.Application.Features.Users.Queries.GetAll;
using Ergo.Application.Persistence;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Tests.Users.Queries
{
    public class GetAllUsersQueryHandlerTests
    {
        private readonly IUserManager _mockUserManager;
        private readonly GetAllUsersQueryHandler _handler;
        public GetAllUsersQueryHandlerTests()
        {
            _mockUserManager = Substitute.For<IUserManager>();

            _handler = new GetAllUsersQueryHandler(_mockUserManager);
        }
        [Fact]
        public async Task GetAllUsersQueryHandler_Success()
        {
            // Arrange
            //Act
            var result = await _handler.Handle(new GetAllUsersQuery(), CancellationToken.None);
            //Assert
            result.Should().NotBeNull();
            result.Users.Should().NotBeEmpty();
            result.Users.Count.Should().Be(2);
        }
    }
}
