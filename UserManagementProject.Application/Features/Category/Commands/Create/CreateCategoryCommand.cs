using MediatR;

namespace UserManagementProject.Application.Features.Category.Commands.Create;

public record CreateCategoryCommand : IRequest<int>
{
    public string Name { get; set; }
}
