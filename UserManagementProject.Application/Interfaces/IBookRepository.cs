using UserManagementProject.Application.Features.Book.DTOs;
using UserManagementProject.Domain.Entities;

namespace UserManagementProject.Application.Interfaces;

public interface IBookRepository : IBaseRepository<Book>
{    
    Task<int> CreateBookAsync(Book book);
    Task<List<BookDto>> GetAllBooksAsync();
    Task<BookDetailsDto?> GetBookDetailsByIdAsync(int bookId);    
    Task UpdateBookAsync(Book book);
}
