using MediatR;
using UserManagementProject.Domain.Enums;

namespace UserManagementProject.Application.Features.User.Commands.Update;

public record UpdateUserCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public string? Surname { get; set; } = string.Empty;
    public List<int>? Roles { get; set; } = new();
}
