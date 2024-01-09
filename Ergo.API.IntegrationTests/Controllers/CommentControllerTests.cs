using Ergo.API.IntegrationTests.Base;
using Ergo.API.IntegrationTests.Dto;
using Ergo.Application.Features.Comments.Commands.CreateComment;
using Ergo.Application.Features.Comments.Queries;
using Ergo.Application.Features.TaskItems.Commands.CreateTaskItem;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Ergo.API.IntegrationTests.Controllers
{
    public class CommentControllerTests : BaseApplicationContextTests
    {
        private const string RequestUri = "/api/v1/comments";

        [Fact]
        public async Task When_GetAllCommentsQueryHandlerIsCalled_Then_Succes()
        {
            string token = CreateToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Act

            var response = await Client.GetAsync(RequestUri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CommentsContainer>(responseString);

            //Assert
            result.Comments?.Count.Should().Be(4);
        }
        [Fact]
       public async Task When_DeleteCommentQuerryHandlerIsCalledWithRightId_Then_Succes()
        {
            //Arrange
            string token = CreateToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.GetAsync($"{RequestUri}");
            var responseString = await response.Content.ReadAsStringAsync();
            var comments = JsonConvert.DeserializeObject<CommentsContainer>(responseString);
            var commentId = comments.Comments.First().CommentId;

            //Act
            response = await Client.DeleteAsync($"{RequestUri}/{commentId}");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CommentDeleteResponse>(responseString);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }

        [Fact]
        public async Task When_GetCommentIdQueryHandlerIsCalledWithRightId_Then_Succes()
        {
            //Arrange
            string token = CreateToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.GetAsync($"{RequestUri}");
            var responseString = await response.Content.ReadAsStringAsync();
            var comments = JsonConvert.DeserializeObject<CommentsContainer>(responseString);


            var commentId = comments.Comments.First().CommentId;

            //Act
            var getResponse = await Client.GetAsync($"{RequestUri}/{commentId}");
            getResponse.EnsureSuccessStatusCode();
            responseString = await getResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CommentResponse>(responseString);

            //Assert
            result.Should().NotBeNull();
            result.Comment.CommentId.Should().Be(commentId);

        }
        [Fact]
        public async Task When_CreateCommentCommandHandlerIsCalledWithRightData_Then_Succes()
        {
            //Arrange
            string token = CreateToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.GetAsync($"{RequestUri}");
            var responseString = await response.Content.ReadAsStringAsync();
            var comments = JsonConvert.DeserializeObject<CommentsContainer>(responseString);
            var commentId = comments.Comments.First().CommentId;

            //Act
            var getResponse = await Client.GetAsync($"{RequestUri}/{commentId}");
            getResponse.EnsureSuccessStatusCode();
            responseString = await getResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CommentResponse>(responseString);

            //Assert
            result.Should().NotBeNull();
            result.Comment.CommentId.Should().Be(commentId);

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
