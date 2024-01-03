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
            context.Projects.AddRange(projects);
            context.SaveChanges();
        }
    }
}
