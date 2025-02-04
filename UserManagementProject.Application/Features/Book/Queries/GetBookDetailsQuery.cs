using MediatR;
using UserManagementProject.Application.Features.Book.DTOs;

namespace UserManagementProject.Application.Features.Book.Queries
{
    public record GetBookDetailsQuery : IRequest<BookDetailsDto>
    {
        public int Id { get; set; }
    }
}
