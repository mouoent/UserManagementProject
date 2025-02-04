using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using UserManagementProject.Application.Features.Category.Commands.Delete;
using UserManagementProject.Application.Features.Category.DTOs;
using UserManagementProject.Application.Interfaces;

namespace UserManagementProject.Tests.UnitTests.Category;

public class DeleteCategoryCommandHandlerTests : TestBase
{
    private readonly DeleteCategoryCommandHandler _handler;
    private readonly IValidator<DeleteCategoryCommand> _validator;

    public DeleteCategoryCommandHandlerTests()
    {        
        _validator = new DeleteCategoryCommandValidator(_categoryRepositoryMock.Object);
        _handler = new DeleteCategoryCommandHandler(_categoryRepositoryMock.Object, _validator);

        // Default setup: Category exists and has no books
        _categoryRepositoryMock
            .Setup(repo => repo.CategoryExistsAsync(It.IsAny<int>()))
            .ReturnsAsync(true);

        _categoryRepositoryMock
            .Setup(repo => repo.GetCategoryDetailsByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new CategoryDetailsDto { Id = 1, Name = "Default", BookIds = new List<int>() });

        _categoryRepositoryMock
            .Setup(repo => repo.GetCategoryByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new CategoryDto { Id = 1, Name = "Default" });

        _categoryRepositoryMock
            .Setup(repo => repo.DeleteCategoryAsync(It.IsAny<int>()))
            .Returns(Task.CompletedTask); // Handling void methods properly
    }

    [Fact]
    public async Task Handle_ShouldCallDeleteCategoryAsync_WhenValidId()
    {
        // Arrange
        var command = new DeleteCategoryCommand { Id = 1 };
      
        _categoryRepositoryMock
            .Setup(repo => repo.DeleteCategoryAsync(command.Id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
        _categoryRepositoryMock.Verify(x => x.DeleteCategoryAsync(1), Times.Once);
    }



    [Fact]
    public async Task Should_HaveError_WhenCategoryIdIsEmpty()
    {
        // Arrange
        var command = new DeleteCategoryCommand { Id = 0 };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Must enter a valid Category ID");
    }

    [Fact]
    public async Task Should_HaveError_WhenCategoryDoesNotExist()
    {
        // Arrange
        var command = new DeleteCategoryCommand { Id = 10 };

        _categoryRepositoryMock
            .Setup(repo => repo.CategoryExistsAsync(command.Id))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Category does not exist.");
    }

    [Fact]
    public async Task Should_HaveError_WhenCategoryHasBooks()
    {
        // Arrange
        var command = new DeleteCategoryCommand { Id = 5 };

        _categoryRepositoryMock
            .Setup(repo => repo.CategoryExistsAsync(command.Id))
            .ReturnsAsync(true);

        _categoryRepositoryMock
            .Setup(repo => repo.GetCategoryDetailsByIdAsync(command.Id))
            .ReturnsAsync(new CategoryDetailsDto
            {
                Id = 5,
                Name = "Sci-Fi",
                BookIds = new List<int> { 1, 2 } // Category has books
            });

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Category is used for one or more books, delete or update these books and try again.");
    }

    [Fact]
    public async Task Should_NotHaveError_WhenValid()
    {
        // Arrange
        var command = new DeleteCategoryCommand { Id = 2 };

        _categoryRepositoryMock
            .Setup(repo => repo.CategoryExistsAsync(command.Id))
            .ReturnsAsync(true);

        _categoryRepositoryMock
            .Setup(repo => repo.GetCategoryDetailsByIdAsync(command.Id))
            .ReturnsAsync(new CategoryDetailsDto
            {
                Id = 2,
                Name = "Fantasy",
                BookIds = new List<int>() // No books, valid case
            });

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
