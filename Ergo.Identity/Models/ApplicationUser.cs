using Ergo.Domain.Entities;
using Ergo.Domain.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace Ergo.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public List<Project>? Projects { get; private set; }
        public List<TaskItem>? Tasks { get; private set; }

        public void UpdateData(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public void AssignProject(Project project)
        {
            if (Projects == null)
            {
                Projects = new List<Project>();
            }
            Projects.Add(project);
        }
        public void AssignTask(TaskItem task)
        {
            if (Tasks == null)
            {
                Tasks = new List<TaskItem>();
            }
            Tasks.Add(task);
        }

    }
}
