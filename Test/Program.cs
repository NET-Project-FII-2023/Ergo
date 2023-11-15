// See https://aka.ms/new-console-template for more information

using Ergo.Application.Features.TaskItems.Commands.UpdateTaskItem;
using Ergo.Application.Features.TaskItems.Queries;
using Ergo.Application.Features.Users.Commands.CreateUser;
using Ergo.Application.Features.Users.Commands.UpdateUser;
using Ergo.Domain.Entities;
using Ergo.Domain.Entities.Enums;
using Infrastructure;
using Infrastructure.Repositories;

Console.WriteLine("Hello, World!");

ErgoContext ergoContext = new ErgoContext();
var user = User.Create( "George", "Rares", "george1@yahoo.com", "1234",UserRole.Developer);
var user2 = User.Create("Denis", "George", "george@yahoo.com", "1234",UserRole.ProjectManager);

//var project = Project.Create("Ergo", "Proiect .NET", DateTime.UtcNow, "George Denis");
//await userRepository.AddAsync(user.Value);
//await projectRepository.AddAsync(project.Value);

UserRepository userRepository = new UserRepository(ergoContext);
ProjectRepository projectRepository = new ProjectRepository(ergoContext);
TaskItemRepository taskItemRepository = new TaskItemRepository(ergoContext);

//CreateUserCommandHandler createUserCommandHandler = new CreateUserCommandHandler(userRepository);
//var createUserCommand = new CreateUserCommand
//{
//    FirstName = "George",
//    Password = "12345667@",
//    LastName = "Rares",
//    Email = "george.duluta@yahoo.com",
//    Role = UserRole.Developer
//};
//var res = await createUserCommandHandler.Handle(createUserCommand, CancellationToken.None);
//Console.WriteLine("Create:" + res.Success);
//UpdateUserCommand updateUserCommand = new UpdateUserCommand
//{
//    UserId = Guid.Parse("0f57cc3f-5003-479b-baac-ed3b16217e8d"),
//    FirstName = "George1212413",
//};
//UpdateUserCommandHandler updateUserCommandHandler = new UpdateUserCommandHandler(userRepository);
//var result = await updateUserCommandHandler.Handle(updateUserCommand, CancellationToken.None);
//Console.WriteLine("Update:" + result.Success);



//user2.Value.AssignProject(project.Value);
//await userRepository.AddAsync(user2.Value);





// --------------------------- Task Item tests --------------------------- //

//var task = TaskItem.Create("Task introductiv", "Create Facebook from scratch", DateTime.UtcNow, "Tudor Paul", Guid.Parse("162f813d-777a-4116-b790-45a151cbafd5"));
//task.Value.AssignUser(user.Value);
//await taskItemRepository.AddAsync(task.Value);

//var taskID = await taskItemRepository.FindByIdAsync(Guid.Parse("e68998a9-1901-4b1a-8877-d18053fcb8bc"));
//Console.WriteLine("Get by id: " + taskID.Value.TaskName);

//var allTasks = await taskItemRepository.GetAllAsync();
//foreach (var item in allTasks.Value)
//{
//    Console.WriteLine("Get all: " + item.TaskName);
//}


//UpdateTaskItemCommand updateTaskItemCommand = new UpdateTaskItemCommand
//{
//    TaskItemId = Guid.Parse("971ed40f-1571-488f-95ef-0e87b4bef8a1"),
//    Description = "Descriere modificata",
//    TaskName = "Nume modificat",
//    Deadline = DateTime.UtcNow.AddDays(3),
//};

//UpdateTaskItemCommandHandler updateTaskItemCommandHandler = new UpdateTaskItemCommandHandler(taskItemRepository);
//var res = await updateTaskItemCommandHandler.Handle(updateTaskItemCommand, CancellationToken.None);
//Console.WriteLine(res.Success);
//if (res.ValidationsErrors != null)
//{
//    foreach (var item in res.ValidationsErrors)
//    {
//        Console.WriteLine(item);
//    }
//}