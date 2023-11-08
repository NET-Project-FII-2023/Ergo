// See https://aka.ms/new-console-template for more information
using Ergo.Domain.Entities;
using Infrastructure;
using Infrastructure.Repositories;

Console.WriteLine("Hello, World!");

ErgoContext ergoContext = new ErgoContext();
var user = User.Create( "George", "Rares", "george1@yahoo.com", "1234");
var user2 = User.Create("Denis", "George", "george@yahoo.com", "1234");

UserRepository userRepository = new UserRepository(ergoContext);
ProjectRepository projectRepository = new ProjectRepository(ergoContext);
TaskItemRepository taskItemRepository = new TaskItemRepository(ergoContext);

//var project = Project.Create("Ergo", "Proiect .NET", DateTime.UtcNow, Guid.Parse("4061db36-f6c8-4016-98a5-d8d8bdd0463b"));
await userRepository.AddAsync(user.Value);

//user2.Value.AssignProject(project.Value);
//await userRepository.AddAsync(user2.Value);


//await projectRepository.AddAsync(project.Value);
//var task = TaskItem.Create("Create Database", "Task introductiv", DateTime.UtcNow, Guid.Parse("4061db36-f6c8-4016-98a5-d8d8bdd0463b"), Guid.Parse("162f813d-777a-4116-b790-45a151cbafd5"));
//await taskItemRepository.AddAsync(task.Value);