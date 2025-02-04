using MediatR;

namespace UserManagementProject.Application.Features.Book.Commands.Update;

public record UpdateBookCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? CategoryId { get; set; }
}
