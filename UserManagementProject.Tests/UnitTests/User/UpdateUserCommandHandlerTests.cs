using FluentValidation.TestHelper;
using Moq;
using UserManagementProject.Application.Features.User.Commands.Update;
using UserManagementProject.Application.Interfaces;

namespace UserManagementProject.Tests.UnitTests.User;

public class UpdateUserCommandValidatorTests
{
    private readonly UpdateUserCommandValidator _validator;

    public UpdateUserCommandValidatorTests()
    {
        _validator = new UpdateUserCommandValidator(Mock.Of<IUserRepository>(), Mock.Of<IRoleRepository>());
    }

    [Fact]
    public async Task Should_HaveError_WhenUserIdIsEmpty()
    {
        // Arrange
        var command = new UpdateUserCommand { Id = 0 };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("User ID is required.");
    }

    [Fact]
    public async Task Should_HaveError_WhenNameIsTooLong()
    {
        // Arrange
        var command = new UpdateUserCommand { Id = 1, Name = new string('A', 101) };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name must be at most 100 characters.");
    }

    [Fact]
    public async Task Should_HaveError_WhenSurnameIsTooLong()
    {
        // Arrange
        var command = new UpdateUserCommand { Id = 1, Surname = new string('B', 101) };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Surname)
            .WithErrorMessage("Surname must be at most 100 characters.");
    }

    [Fact]
    public async Task Should_HaveError_WhenRolesAreEmpty()
    {
        // Arrange
        var command = new UpdateUserCommand { Id = 1, Roles = new List<int>() };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Roles)
            .WithErrorMessage("User must have at least one role.");
    }

    [Fact]
    public async Task Should_NotHaveError_WhenValidData()
    {
        // Arrange
        var command = new UpdateUserCommand
        {
            Id = 1,
            Name = "Valid Name",
            Surname = "Valid Surname",
            Roles = new List<int> { 2 }
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
        result.ShouldNotHaveValidationErrorFor(x => x.Surname);
        result.ShouldNotHaveValidationErrorFor(x => x.Roles);
    }
}