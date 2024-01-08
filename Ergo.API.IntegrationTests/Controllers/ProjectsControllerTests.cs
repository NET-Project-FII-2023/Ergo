using Ergo.API.IntegrationTests.Base;
using Ergo.API.IntegrationTests.Dto;
using Ergo.Application.Features.Projects.Commands.CreateProject;
using Ergo.Application.Features.Projects.Queries;
using FluentAssertions;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Ergo.API.IntegrationTests.Controllers
{
    public class ProjectsControllerTests : BaseApplicationContextTests
    {
        private const string RequestUri = "/api/v1/projects";

        [Fact]
        public async Task When_GetAllProjectsQueryHandlerIsCalled_Then_Success()
        {
            //Arrange
            string token = CreateToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); 
            //Act
            var response = await Client.GetAsync(RequestUri);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProjectsContainer>(responseString);
            // Assert
            result.Projects?.Count.Should().Be(4); 

        }
        [Fact]
        public async Task When_PostProjectCommandHandlerIsCalledWithRightParameters_Then_Success()
        {
            //Arrange
            string token = CreateToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var project = new CreateProjectCommand
            {
                ProjectName = "TestProject2",
                Description = "TestDescription",
                GitRepository = "TestGitRepository",
                FullName = "john_doe",
                Deadline = DateTime.Now.AddDays(1)
            };
            //Act
            var response = await Client.PostAsJsonAsync(RequestUri, project);

            //Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProjectResponse>(responseString);
            result?.Should().NotBeNull();
            result?.Project.ProjectName.Should().Be(project.ProjectName);

        }
        [Fact]
        public async Task When_GetProjectByIdQueryHandlerIsCalledWithRightId_Then_Success()
        {
            //Arrange
            string token = CreateToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.GetAsync(RequestUri);
            var responseString = await response.Content.ReadAsStringAsync();
            var projects = JsonConvert.DeserializeObject<ProjectsContainer>(responseString);
            var projectId = projects.Projects.First().ProjectId;
            //Act
            var getResponse = await Client.GetAsync($"{RequestUri}/{projectId}");
            getResponse.EnsureSuccessStatusCode();
            responseString = await getResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProjectDto>(responseString);
            // Assert
            result.Should().NotBeNull();
            result.ProjectId.Should().Be(projectId);
        }
        [Fact]
        public async Task When_DeleteProjectQueryHandlerIsCalledWithRightId_Then_Success()
        {
            //Arrange
            string token = CreateToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.GetAsync(RequestUri);
            var responseString = await response.Content.ReadAsStringAsync();
            var projects = JsonConvert.DeserializeObject<ProjectsContainer>(responseString);
            var projectId = projects.Projects.First().ProjectId;
            //Act
            response = await Client.DeleteAsync($"{RequestUri}/{projectId}");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProjectDeleteResponse>(responseString);
            // Assert
            result.Should().NotBeNull();
            result?.Success.Should().Be(true);
        }
        private static string CreateToken()
        {

            return JwtTokenProvider.JwtSecurityTokenHandler.WriteToken(
            new JwtSecurityToken(
                JwtTokenProvider.Issuer,
                JwtTokenProvider.Issuer,
                new List<Claim> { new(ClaimTypes.Role, "User")},
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: JwtTokenProvider.SigningCredentials
            ));
        }
    }
}
