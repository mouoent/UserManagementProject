using Microsoft.EntityFrameworkCore;
using UserManagementProject.Application.Features.Book.DTOs;
using UserManagementProject.Application.Interfaces;
using UserManagementProject.Domain.Entities;
using UserManagementProject.Infrastructure.Persistence;

namespace UserManagementProject.Infrastructure.Repositories;

public class BookRepository : BaseRepository<Book>, IBookRepository
{    
    public BookRepository(UserManagementDbContext context, IAuditLogService auditLogService)
          : base(context, auditLogService) { }

    public async Task<int> CreateBookAsync(Book book)
    {
        return await CreateAsync(book);
    }

    public async Task<BookDetailsDto?> GetBookDetailsByIdAsync(int bookId)
    {
        var result = await _context.Books
            .AsNoTracking() 
            .Where(b => b.Id == bookId)
            .Select(b => new BookDetailsDto
            {
                Id = b.Id,
                Name = b.Name,
                CategoryId = b.Category.Id 
            })
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<List<BookDto>> GetAllBooksAsync()
    {
        var result = await _context.Books
            .AsNoTracking()
            .Select(b => new BookDto
            {
                Id = b.Id,
                Name = b.Name,
            })
        .ToListAsync();
        return result;
    }

    public async Task UpdateBookAsync(Book book)
    {
        await UpdateAsync(book);
    }
}
