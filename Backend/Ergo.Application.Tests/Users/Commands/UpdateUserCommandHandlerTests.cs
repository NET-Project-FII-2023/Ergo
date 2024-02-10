using NSubstitute;
using FluentAssertions;
using Ergo.Application.Features.Users.Commands.UpdateUser;
using Ergo.Application.Persistence;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;
using Xunit;
using Ergo.Domain.Entities;
using Ergo.Application.Features.Users.Queries;
using Ergo.Domain.Common;

public class UpdateUserCommandHandlerTests
{
    private readonly IUserManager _userRepository;
    private readonly IUserPhotoRepository _userPhotoRepository;
    private readonly UpdateUserCommandHandler _handler;

    public UpdateUserCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserManager>();
        _userPhotoRepository = Substitute.For<IUserPhotoRepository>();
        _handler = new UpdateUserCommandHandler(_userRepository,_userPhotoRepository);
    }

    [Fact]
    public async Task Handle_UserExists_UpdatesSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new UpdateUserCommand { Id = userId, Name = "New Name", Username = "NewUsername", Email = "newemail@example.com" };

        var existingUser = Result<UserDto>.Success(new UserDto { UserId = userId.ToString() }); 
        _userRepository.FindByIdAsync(userId).Returns(Task.FromResult(existingUser));

        _userRepository.FindByEmailAsync(request.Email).Returns(Task.FromResult(Result<UserDto>.Failure("Not found")));
        _userRepository.FindByUsernameAsync(request.Username).Returns(Task.FromResult(Result<UserDto>.Failure("Not found")));

        _userRepository.UpdateAsync(Arg.Any<UserDto>()).Returns(Task.FromResult(Result<UserDto>.Success(new UserDto())));

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Success.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_UpdateFails_ReturnsErrorResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new UpdateUserCommand { Id = userId, Name = "Updated Name", Username = "UpdatedUsername", Email = "updatedemail@example.com" };

        var existingUser = Result<UserDto>.Success(new UserDto { UserId = userId.ToString(), Name = "Original Name", Username = "OriginalUsername", Email = "originalemail@example.com" });
        _userRepository.FindByIdAsync(userId).Returns(Task.FromResult(existingUser));

        // Presupunem că nu există conflicte de email sau username
        _userRepository.FindByEmailAsync(request.Email).Returns(Task.FromResult(Result<UserDto>.Failure("Not found")));
        _userRepository.FindByUsernameAsync(request.Username).Returns(Task.FromResult(Result<UserDto>.Failure("Not found")));

        // Configurăm repository-ul să returneze un eșec la update
        _userRepository.UpdateAsync(Arg.Any<UserDto>()).Returns(Task.FromResult(Result<UserDto>.Failure("Update failed")));

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Success.Should().BeFalse();
        response.ValidationsErrors.Should().Contain("Update failed");
    }


    [Fact]
    public async Task Handle_UserDoesNotExist_ReturnsErrorResponse()
    {
        // Arrange
        var request = new UpdateUserCommand { Id = Guid.NewGuid() };
        _userRepository.FindByIdAsync(request.Id).Returns(Task.FromResult(Result<UserDto>.Failure("User not found")));

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Success.Should().BeFalse();
        response.ValidationsErrors.Should().Contain("User with id this does not exists");
    }


    [Fact]
    public async Task Handle_ValidationFails_ReturnsErrorResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new UpdateUserCommand
        {
            Id = userId,
            Name = "", 
            Username = "invalid username!", 
            Email = "invalid-email" 
        };

        var existingUser = Result<UserDto>.Success(new UserDto { UserId = userId.ToString() });
        _userRepository.FindByIdAsync(userId).Returns(Task.FromResult(existingUser)); // Asigurați că aveți un rezultat pentru acest apel

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Success.Should().BeFalse();
        response.ValidationsErrors.Should().NotBeEmpty();
    }



}