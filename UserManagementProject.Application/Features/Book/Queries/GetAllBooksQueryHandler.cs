using MediatR;
using UserManagementProject.Application.Features.Book.DTOs;
using UserManagementProject.Application.Interfaces;

namespace UserManagementProject.Application.Features.Book.Queries;

public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, List<BookDto>>
{
    private readonly IBookRepository _bookRepository;

    public GetAllBooksQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<List<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        return await _bookRepository.GetAllBooksAsync();
    }
}
