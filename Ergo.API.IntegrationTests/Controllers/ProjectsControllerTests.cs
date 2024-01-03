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
        public async Task When_PostProjectCommandHandlerIsCalledWithRightParameters_Then_TheEntityCreatedShouldBeReturned()
        {
            //Arrange
            string token = CreateToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var project = new CreateProjectCommand
            {
                ProjectName = "TestProject",
                Description = "TestDescription",
                GitRepository = "TestGitRepository",
                FullName = "TestFullName",
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
