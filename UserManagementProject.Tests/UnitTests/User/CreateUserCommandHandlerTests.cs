using FluentValidation;
using Moq;
using UserManagementProject.Application.Features.User.Commands.Create;

namespace UserManagementProject.Tests.UnitTests.User;

public class CreateUserCommandHandlerTests : TestBase
{
    private readonly CreateUserCommandHandler _handler;
    private readonly IValidator<CreateUserCommand> _validator;

    public CreateUserCommandHandlerTests()
    {
        _validator = new CreateUserCommandValidator(_roleRepositoryMock.Object);
        _handler = new CreateUserCommandHandler(_userRepositoryMock.Object, _roleRepositoryMock.Object, _validator);

        // Setup mock repository
        _roleRepositoryMock
            .Setup(repo => repo.GetRolesByIdsAsync(It.IsAny<List<int>>()))
            .ReturnsAsync(new List<UserManagementProject.Domain.Entities.Role>
            {
                new UserManagementProject.Domain.Entities.Role { Id = 1, Name = "User" }
            });

        _roleRepositoryMock
            .Setup(repo => repo.GetAllRolesAsync())
            .ReturnsAsync(new List<UserManagementProject.Domain.Entities.Role>
            {
                new UserManagementProject.Domain.Entities.Role { Id = 1, Name = "Admin" },
                new UserManagementProject.Domain.Entities.Role { Id = 2, Name = "User" }
            });

        _userRepositoryMock
            .Setup(repo => repo.CreateUserAsync(It.IsAny<UserManagementProject.Domain.Entities.User>()))
            .ReturnsAsync(1);

    }

    [Fact]
    public async Task Handle_ShouldCreateUser_WhenValidRequest()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Name = "Tony",
            Surname = "Karou",
            Roles = new List<int> { 1 }
        };        

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(1, result);
        _userRepositoryMock.Verify(x => x.CreateUserAsync(It.IsAny<UserManagementProject.Domain.Entities.User>()), Times.Once);
    }
}