using Ergo.API.IntegrationTests.Base;
using Ergo.API.IntegrationTests.Dto;
using Ergo.Application.Features.InboxItems.Commands.CreateInboxItem;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.API.IntegrationTests.Controllers
{
    public class InboxItemControllerTests : BaseApplicationContextTests
    {
        private const string RequestUri = "/api/v1/inboxitem";
        [Fact]
        public async Task When_CreateInboxItemCommandHandlerIsCalled_Then_Success()
        {
            //Arrange
            string token = CreateToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var inboxItem = new CreateInboxItemCommand
            {
                Message = "new message",
                UserId = Guid.NewGuid()
            };
            //Act
            var response = await Client.PostAsJsonAsync(RequestUri, inboxItem);
            //Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CreateInboxItemCommandResponse>(responseString);
            result?.Should().NotBeNull();
            result?.InboxItem.Message.Should().Be(inboxItem.Message);
            
        }
        [Fact]
        public async Task When_GetAllInboxItemsQueryHandlerIsCalled_Then_Success()
        {
            //Arrange
            string token = CreateToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Act
            var response = await Client.GetAsync(RequestUri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<InboxItemsResponse>(responseString);
            // Assert
            result.InboxItems?.Count.Should().Be(4);

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
