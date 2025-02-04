using MediatR;

namespace UserManagementProject.Application.Features.Book.Commands.Create;

public record CreateBookCommand : IRequest<int>
{
    public string Name { get; set; }
    public int CategoryId { get; set; }
}
