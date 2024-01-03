using Ergo.API.IntegrationTests.Base;
using Ergo.API.IntegrationTests.Dto;
using Ergo.Application.Features.TaskItems.Commands.CreateTaskItem;
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
            var response = await Client.GetAsync(RequestUri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TasksContainer>(responseString);
            // Assert
            result.TaskItems?.Count.Should().Be(4);
        }
        [Fact]
        public async Task When_PostTaskCommandHandlerIsCalledWithRightParameters_Then_TheEntityCreatedShouldBeReturned()
        {
            //Arrange
            string token = CreateToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var task = new CreateTaskItemCommand
            {
                TaskName = "TestProject",
                Description = "TestDescription",
                Deadline = DateTime.Now.AddDays(1),
                CreatedBy = "TestFullName",
                ProjectId = Guid.Parse("b2d9a0a0-0b7a-4e6a-8b0a-08d96d0b8b0a"),
                State = TaskState.ToDo
            };
            //Act
            var response = await Client.PostAsJsonAsync(RequestUri, task);

            //Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TaskItemResponse>(responseString);
            result?.Should().NotBeNull();
            result?.TaskItem.ProjectId.Should().Be(task.ProjectId);
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
