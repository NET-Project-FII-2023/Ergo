// See https://aka.ms/new-console-template for more information

using Ergo.Application.Features.TaskItems.Queries;
using Ergo.Domain.Entities;
using Ergo.Domain.Entities.Enums;
using Infrastructure;
using Infrastructure.Repositories;

Console.WriteLine("Hello, World!");

ErgoContext ergoContext = new ErgoContext();
var user = User.Create( "George", "Rares", "george1@yahoo.com", "1234",UserRole.Developer);
var user2 = User.Create("Denis", "George", "george@yahoo.com", "1234",UserRole.ProjectManager);



UserRepository userRepository = new UserRepository(ergoContext);
ProjectRepository projectRepository = new ProjectRepository(ergoContext);
TaskItemRepository taskItemRepository = new TaskItemRepository(ergoContext);

var project = Project.Create("Ergo", "Proiect .NET", DateTime.UtcNow, "George Denis");
await userRepository.AddAsync(user.Value);

//user2.Value.AssignProject(project.Value);
//await userRepository.AddAsync(user2.Value);


await projectRepository.AddAsync(project.Value);

// --------------------------- Task Item tests --------------------------- //

//var task = TaskItem.Create("DO something", "Task introductiv", DateTime.UtcNow, "Tudor Paul", Guid.Parse("162f813d-777a-4116-b790-45a151cbafd5"));
//task.Value.AssignUser(user.Value);
//await taskItemRepository.AddAsync(task.Value);

//var task2 = TaskItem.Create("Exploateaza BD-ul", "Fa ceva", DateTime.UtcNow, "Tudor Paul Stroe", Guid.Parse("162f813d-777a-4116-b790-45a151cbafd5"));
//task2.Value.AssignUser(user.Value);
//await taskItemRepository.AddAsync(task2.Value);

//var taskID = await taskItemRepository.FindByIdAsync(Guid.Parse("e68998a9-1901-4b1a-8877-d18053fcb8bc"));
//Console.WriteLine("Get by id: "+ taskID.Value.TaskName);

//var allTasks = await taskItemRepository.GetAllAsync();
//foreach (var item in allTasks.Value)
//{
//    Console.WriteLine("Get all: " + item.TaskName);
//}