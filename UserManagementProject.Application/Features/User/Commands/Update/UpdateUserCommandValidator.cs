using FluentValidation;
using UserManagementProject.Application.Interfaces;

namespace UserManagementProject.Application.Features.User.Commands.Update;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Name must be at most 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Name)); 

        RuleFor(x => x.Surname)
            .MaximumLength(100).WithMessage("Surname must be at most 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Surname)); 

        RuleFor(x => x.Roles)
              .NotEmpty().WithMessage("User must have at least one role.")
              .When(x => x.Roles is not null); 
    }

}
