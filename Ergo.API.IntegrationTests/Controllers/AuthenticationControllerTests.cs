using Ergo.API.IntegrationTests.Base;
using Ergo.Application.Models.Identity;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Ergo.API.IntegrationTests.Controllers
{
    public class AuthenticationControllerTests : BaseApplicationContextTests
    {
        private const string RequestUri = "/api/v1/Authentication";
        [Fact]
        public async Task When_RegisterIsCalled_Then_Success()
        {
            //Arrange
            var registrationModel = new RegistrationModel
            {
                Username = "Johnny",
                Name = "Johnny",
                Email = "johnny@yahoo.com",
                Password = "Abc123@",
            };
            //Act
            var response = await Client.PostAsJsonAsync($"{RequestUri}/register", registrationModel);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<RegistrationModel>(responseString);
            // Assert
            result?.Should().NotBeNull();
            result?.Username.Should().Be(registrationModel.Username);
        }
        [Fact]
        public async Task When_RegisterIsCalledWithExistingUsername_Then_Fail()
        {
            //Arrange
            var registrationModel = new RegistrationModel
            {
                Username = "john_doe",
                Name = "Johnny",
                Email = "john.doe@example.com",
                Password = "Abc123@",
            };
            //Act
            var response = await Client.PostAsJsonAsync($"{RequestUri}/register", registrationModel);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var message = await response.Content.ReadAsStringAsync();
            // Assert
            message.Should().Be("User already exists");
        }
        [Fact]
        public async Task When_RegisterIsCalledWithWithExisting_Then_Fail()
        {
            //Arrange
            var registrationModel = new RegistrationModel
            {
                Username = "johnny",
                Name = "Johnny",
                Email = "john.doe@example.com",
                Password = "Abc123@",
            };
            //Act
            var response = await Client.PostAsJsonAsync($"{RequestUri}/register", registrationModel);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var message = await response.Content.ReadAsStringAsync();
            // Assert
            message.Should().Be("User with this email already exists");
        }
        [Fact]
        public async Task When_RegisterIsCalledWithInvalidPassword_Then_Fail()
        {
            //Arrange
            var registrationModel = new RegistrationModel
            {
                Username = "johnny",
                Name = "Johnny",
                Email = "john@yahoo.com",
                Password = "Abc12",
            };
            //Act
            var response = await Client.PostAsJsonAsync($"{RequestUri}/register", registrationModel);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var message = await response.Content.ReadAsStringAsync();
            // Assert
            message.Should().Be("Password is not valid! The password must have at least 7 characters and needs to include a capital letter, a symbol, a digit.");
        }
       
        [Fact]
        public async Task When_LoginIsCalled_Then_Success()
        {
            //Arrange
            var loginModel = new LoginModel
            {
                Username = "john_doe",
                Password = "ComplexPass1!",
            };
            //Act
            var response = await Client.PostAsJsonAsync($"{RequestUri}/login", loginModel);
            response.EnsureSuccessStatusCode();
            var token = await response.Content.ReadAsStringAsync();
            // Assert
            token?.Should().NotBeNull();
            token.Should().NotBe("Invalid username");
            token.Should().NotBe("Invalid password");
        }
        [Fact]
        public async Task When_LoginIsCalledWithInvalidUsername_Then_Fail()
        {
            //Arrange
            var loginModel = new LoginModel
            {
                Username = "john_doe1",
                Password = "ComplexPass1!",
            };
            //Act
            var response = await Client.PostAsJsonAsync($"{RequestUri}/login", loginModel);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var token = await response.Content.ReadAsStringAsync();
            // Assert
            token.Should().Be("Invalid username");
        }
        [Fact]
        public async Task When_LoginIsCalledWithInvalidPassword_Then_Fail()
        {
            //Arrange
            var loginModel = new LoginModel
            {
                Username = "john_doe",
                Password = "ComplexPass1",
            };
            //Act
            var response = await Client.PostAsJsonAsync($"{RequestUri}/login", loginModel);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var token = await response.Content.ReadAsStringAsync();
            // Assert
            token.Should().Be("Invalid password");
        }
    }
}
