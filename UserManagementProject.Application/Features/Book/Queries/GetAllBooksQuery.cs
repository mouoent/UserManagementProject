using MediatR;
using UserManagementProject.Application.Features.Book.DTOs;

namespace UserManagementProject.Application.Features.Book.Queries;

public record GetAllBooksQuery : IRequest<List<BookDto>> { }
