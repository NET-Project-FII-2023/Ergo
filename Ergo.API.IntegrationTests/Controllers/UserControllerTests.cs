using Ergo.API.IntegrationTests.Base;
using Ergo.API.IntegrationTests.Dto;
using FluentAssertions;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Ergo.API.IntegrationTests.Controllers
{
    public class UserControllerTests : BaseApplicationContextTests
    {
        private const string RequestUri = "/api/v1/Users";
        [Fact]
        public async Task When_GetAllUsersQueryHandlerIsCalled_Then_Success()
        {
            //Arrange
            string token = CreateToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Act
            var response = await Client.GetAsync(RequestUri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UsersContainer>(responseString);
            // Assert
            result.Users?.Count.Should().Be(2);
        }
        [Fact]
        public async Task When_GetUserByEmailQueryHandlerIsCalled_Then_Success()
        {
            //Arrange
            string token = CreateToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Act
            var response = await Client.GetAsync($"{RequestUri}/ByEmail/john.doe@example.com");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserResponse>(responseString);
            // Assert
            result.User.Should().NotBeNull();
            result.User.Email.Should().Be("john.doe@example.com");
        }
        private static string CreateToken()
        {

            return JwtTokenProvider.JwtSecurityTokenHandler.WriteToken(
            new JwtSecurityToken(
                JwtTokenProvider.Issuer,
                JwtTokenProvider.Issuer,
                new List<Claim> { new(ClaimTypes.Role, "User") },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: JwtTokenProvider.SigningCredentials
            ));
        }
    }
}
