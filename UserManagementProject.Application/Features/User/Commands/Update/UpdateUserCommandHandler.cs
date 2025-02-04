using FluentValidation;
using MediatR;
using UserManagementProject.Application.Interfaces;

namespace UserManagementProject.Application.Features.User.Commands.Update;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IValidator<UpdateUserCommand> _validator;

    public UpdateUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IValidator<UpdateUserCommand> validator)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _validator = validator;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        // Validate command
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        // Fetch user entity
        var user = await _userRepository.GetEntityByIdAsync(request.Id);
        if (user is null)
            throw new KeyNotFoundException("User not found");

        // Fetch roles if provided 
        if (request.Roles is not null && request.Roles.Any())
        {
            var roles = await _roleRepository.GetRolesByIdsAsync(request.Roles);
            if (roles.Count != request.Roles.Count)
                throw new KeyNotFoundException("One or more roles were not found");

            user.Roles = roles; // Update roles
        }

        // Apply updates
        user.Name = request.Name ?? user.Name;
        user.Surname = request.Surname ?? user.Surname;
        
        await _userRepository.UpdateUserAsync(user);
        return Unit.Value;
    }
}
