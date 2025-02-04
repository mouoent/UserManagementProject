using MediatR;

namespace UserManagementProject.Application.Features.Category.Commands.Delete;

public record DeleteCategoryCommand : IRequest<Unit>
{
    public int Id { get; set; }
}
