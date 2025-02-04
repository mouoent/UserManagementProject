using MediatR;
using UserManagementProject.Domain.Enums;

namespace UserManagementProject.Application.Features.User.Commands.Create;

public record CreateUserCommand : IRequest<int>
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public List<int> Roles { get; set; } = new();
}
