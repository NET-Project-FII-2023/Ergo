using Ergo.Domain.Entities;
using Infrastructure;

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
            context.TaskItems.AddRange(tasks);
            context.Projects.AddRange(projects);
            context.SaveChanges();
        }
    }
}
