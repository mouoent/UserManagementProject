using FluentValidation;
using MediatR;
using UserManagementProject.Application.Interfaces;
using UserManagementProject.Domain.Entities;
using UserManagementProject.Domain.Enums;

namespace UserManagementProject.Application.Features.User.Commands.Create;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IValidator<CreateUserCommand> _validator;

    public CreateUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IValidator<CreateUserCommand> validator)
    {
        _userRepository = userRepository;
        _validator = validator;
        _roleRepository = roleRepository;
    }

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Validate command
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        // Get roles from db to store in new user
        var roles = await _roleRepository.GetRolesByIdsAsync(request.Roles);

        var user = new Domain.Entities.User
        {
            Name = request.Name,
            Surname = request.Surname,
            Roles = roles
        };

        var result = await _userRepository.CreateUserAsync(user);
        return result;
    }
}
