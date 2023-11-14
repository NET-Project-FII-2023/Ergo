// See https://aka.ms/new-console-template for more information
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



UserRepository userRepository = new UserRepository(ergoContext);
ProjectRepository projectRepository = new ProjectRepository(ergoContext);
TaskItemRepository taskItemRepository = new TaskItemRepository(ergoContext);

var project = Project.Create("Ergo", "Proiect .NET", DateTime.UtcNow, "George Denis");
//await userRepository.AddAsync(user.Value);

//user2.Value.AssignProject(project.Value);?
//await userRepository.AddAsync(user2.Value);


//await projectRepository.AddAsync(project.Value);

var task = TaskItem.Create("Create Database", "Task introductiv", DateTime.UtcNow, "Tudor Paul", Guid.Parse("162f813d-777a-4116-b790-45a151cbafd5"));
//await taskItemRepository.AddAsync(task.Value);
//CreateUserCommandHandler createUserCommandHandler = new CreateUserCommandHandler(userRepository);
//var createUserCommand = new CreateUserCommand
//{
//    FirstName = "George",
//    Password = "12345667@",
//    LastName = "Rares",
//    Email = "george.duluta@yahoo.com",
//    Role = UserRole.Developer
//};
//var result = await createUserCommandHandler.Handle(createUserCommand, CancellationToken.None);
//Console.WriteLine(result.Success);
UpdateUserCommand updateUserCommand = new UpdateUserCommand
{
    UserId = Guid.Parse("cde7db19-1d1e-4c89-821b-7cdd4a1d0a18"),
    FirstName = "George123",

};
UpdateUserCommandHandler updateUserCommandHandler = new UpdateUserCommandHandler(userRepository);
var result = await updateUserCommandHandler.Handle(updateUserCommand, CancellationToken.None);
Console.WriteLine(result.Success);

