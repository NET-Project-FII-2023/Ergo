using Ergo.Application.Models.Identity;
using Ergo.Application.Persistence;
using Ergo.Domain.Entities;
using Ergo.Identity;
using Ergo.Identity.Models;
using Ergo.Identity.Services;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Ergo.API.IntegrationTests.Base
{
    public class Seed
    {
        public static void InitializeDbForTests(ErgoContext context)
        {
            var projects = new List<Project>
            {
                Project.Create("Ergo", ".NET", null, DateTime.UtcNow, "John").Value,
                Project.Create("Labs", ".NET", null, DateTime.UtcNow, "George").Value,
                Project.Create("Github", "Angular", null, DateTime.UtcNow, "John").Value,
                Project.Create("LFAC", "C", null, DateTime.UtcNow, "Josh").Value,
            };
            var tasks = new List<TaskItem>
            {
                TaskItem.Create("Ergo", "Create project", DateTime.UtcNow,"John",Guid.NewGuid()).Value,
                TaskItem.Create("Labs", "Create project", DateTime.UtcNow,"George",Guid.NewGuid()).Value,
                TaskItem.Create("Github", "Create project", DateTime.UtcNow,"John",Guid.NewGuid()).Value,
                TaskItem.Create("LFAC", "Create project", DateTime.UtcNow,"Josh",Guid.NewGuid()).Value,

            };
            var users = new List<User>
            {
                User.Create(Guid.NewGuid()).Value,
                User.Create(Guid.NewGuid()).Value,
                User.Create(Guid.NewGuid()).Value,
                User.Create(Guid.NewGuid()).Value,
            };
            var comments = new List<Comment>
            {
                Comment.Create("Mihai",Guid.NewGuid(),"comment").Value,
                Comment.Create("Mihai2",Guid.NewGuid(),"comment2").Value,
                Comment.Create("Mihai3",Guid.NewGuid(),"comment3").Value,
                Comment.Create("Mihai4",Guid.NewGuid(),"comment4").Value,

            };
            var inboxItem = new List<InboxItem>
            {
                InboxItem.Create(Guid.NewGuid(),"mesaj").Value,
                InboxItem.Create(Guid.NewGuid(),"mesaj2").Value,
                InboxItem.Create(Guid.NewGuid(),"mesaj3").Value,
                InboxItem.Create(Guid.NewGuid(),"mesaj4").Value,
                

            };

            context.TaskItems.AddRange(tasks);
            context.Projects.AddRange(projects);
            context.Users.AddRange(users);
            context.Comments.AddRange(comments);
            context.SaveChanges();

        }
        public static async Task InitializeUserDbForTests(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IUserRepository userRepository, IPasswordResetCode passwordResetCode)
        {
            var configuration = new ConfigurationBuilder().Build(); 
            var authService = new AuthService(userManager, roleManager, configuration, null, userRepository,passwordResetCode); 

            var registrationModels = new List<RegistrationModel>
        {
            new RegistrationModel{ Username = "john_doe", Email = "john.doe@example.com", Name = "John Doe", Password = "ComplexPass1!"},
            new RegistrationModel{ Username = "jane_doe", Email = "jane_doe@example.com", Name = "Jane Doe", Password = "ComplexPass1!"},

        };

            foreach (var model in registrationModels)
            {
                var (status, message) = await authService.Registeration(model, UserRoles.User);
                if (status == 0)
                {
                    Console.WriteLine($"Failed to create user: {message}");
                }
            }
        }
    }
}
