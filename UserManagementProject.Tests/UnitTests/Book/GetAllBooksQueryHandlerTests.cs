using Moq;
using UserManagementProject.Application.Features.Book.DTOs;
using UserManagementProject.Application.Features.Book.Queries;

namespace UserManagementProject.Tests.UnitTests.Book;

public class GetAllBooksQueryHandlerTests : TestBase
{
    private readonly GetAllBooksQueryHandler _handler;

    public GetAllBooksQueryHandlerTests()
    {
        _handler = new GetAllBooksQueryHandler(_bookRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnBooks_WhenBooksExist()
    {
        // Arrange
        var fakeBooks = new List<BookDto>
    {
        new() { Id = 1, Name = "Book 1" },
        new() { Id = 2, Name = "Book 2" }
    };

        _bookRepositoryMock.Setup(repo => repo.GetAllBooksAsync())
            .ReturnsAsync(fakeBooks);

        // Act
        var result = await _handler.Handle(new GetAllBooksQuery(), CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        _bookRepositoryMock.Verify(x => x.GetAllBooksAsync(), Times.Once);
    }
}
