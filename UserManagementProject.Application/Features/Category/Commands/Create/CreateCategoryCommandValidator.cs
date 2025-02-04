using FluentValidation;
using UserManagementProject.Application.Features.Category.Commands.Create;
using UserManagementProject.Application.Interfaces;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required")
            .MaximumLength(100).WithMessage("Category name must be at most 100 characters")
            .MustAsync(BeUniqueCategory).WithMessage("Category name must be unique");
    }

    private async Task<bool> BeUniqueCategory(string categoryName, CancellationToken cancellationToken)
    {
        return !await _categoryRepository.CategoryExistsAsync(categoryName);
    }
}
