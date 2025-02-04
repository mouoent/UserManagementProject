using FluentValidation;
using UserManagementProject.Application.Features.User.Commands.Create;
using UserManagementProject.Domain.Enums;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IRoleRepository _roleRepository;

    public CreateUserCommandValidator(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must be at most 100 characters");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Surname is required")
            .MaximumLength(100).WithMessage("Name must be at most 100 characters");

        RuleFor(x => x.Roles)
            .NotEmpty().WithMessage("User must have at least one role")
            .MustAsync(ValidRoles).WithMessage("One or more roles are invalid");
    }

    private async Task<bool> ValidRoles(List<int> roles, CancellationToken cancellationToken)
    {
        if (roles == null || !roles.Any())
            return false;

        var validRoles = await _roleRepository.GetAllRolesAsync();
        return roles.All(role => validRoles.Any(r => r.Id == role));
    }
}
