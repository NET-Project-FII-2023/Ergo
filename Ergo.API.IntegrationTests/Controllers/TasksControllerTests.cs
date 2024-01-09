using Ergo.API.IntegrationTests.Base;
using Ergo.API.IntegrationTests.Dto;
using Ergo.Application.Features.TaskItems.Commands.CreateTaskItem;
using Ergo.Application.Features.TaskItems.Queries;
using Ergo.Domain.Common;
using Ergo.Domain.Entities;
using Ergo.Domain.Entities.Enums;
using FluentAssertions;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Ergo.API.IntegrationTests.Controllers
{
    public class TasksControllerTests : BaseApplicationContextTests
    {
        private const string RequestUri = "/api/v1/TaskItems";
        [Fact]
        public async Task When_GetAllTasksQueryHandlerIsCalled_Then_Success()
        {
            //Arrange
            string token = CreateToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Act
            var response = await Client.GetAsync    (RequestUri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TasksContainer>(responseString);
            // Assert
            result.TaskItems?.Count.Should().Be(4);
        }
        [Fact]
        public async Task When_GetTaskByIdQueryHandlerIsCalledWithRightId_Then_Success()
        {
            //Arrange
            string token = CreateToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.GetAsync($"{RequestUri}");
            var responseString = await response.Content.ReadAsStringAsync();
            var tasks = JsonConvert.DeserializeObject<TasksContainer>(responseString);
            var taskId = tasks.TaskItems.First().TaskItemId;
            //Act
            var getResponse = await Client.GetAsync($"{RequestUri}/{taskId}");
            getResponse.EnsureSuccessStatusCode();
            responseString = await getResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TaskItemResponse>(responseString);
            // Assert
            result.Should().NotBeNull();
            result.TaskItem.TaskItemId.Should().Be(taskId);
        }

       
        
        [Fact]
        public async Task When_DeleteTaskQueryHandlerIsCalledWithRightId_Then_Success()
        {
            //Arrange
            string token = CreateToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.GetAsync($"{RequestUri}");
            var responseString = await response.Content.ReadAsStringAsync();
            var tasks = JsonConvert.DeserializeObject<TasksContainer>(responseString);
            var taskId = tasks.TaskItems.First().TaskItemId;
            //Act
            var deleteResponse = await Client.DeleteAsync($"{RequestUri}/{taskId}");
            deleteResponse.EnsureSuccessStatusCode();
            responseString = await deleteResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TaskDeleteResponse>(responseString);
            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
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
