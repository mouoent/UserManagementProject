using MediatR;
using UserManagementProject.Application.Features.Book.DTOs;
using UserManagementProject.Application.Interfaces;

namespace UserManagementProject.Application.Features.Book.Queries;

public class GetBookDetailsQueryHandler : IRequestHandler<GetBookDetailsQuery, BookDetailsDto>
{
    private IBookRepository _bookRepository;

    public GetBookDetailsQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookDetailsDto> Handle(GetBookDetailsQuery request, CancellationToken cancellationToken)
    {
        var result = await _bookRepository.GetBookDetailsByIdAsync(request.Id);

        if (result is null)
            throw new KeyNotFoundException("Book not found");

        return result;
    }
}
