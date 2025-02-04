using FluentValidation;
using UserManagementProject.Application.Features.Book.Commands.Update;
using UserManagementProject.Application.Interfaces;

public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{    
    public UpdateBookCommandValidator(ICategoryRepository categoryRepository)
    {
        RuleFor(x => x.Id)
           .NotEmpty().WithMessage("Book ID is required.");

        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Name must be at most 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));        
    }
}
