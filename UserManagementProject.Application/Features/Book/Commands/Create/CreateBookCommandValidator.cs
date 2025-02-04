using FluentValidation;
using UserManagementProject.Application.Features.Book.Commands.Create;
using UserManagementProject.Application.Interfaces;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateBookCommandValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Book name is required")
            .MaximumLength(255).WithMessage("Book name must be at most 255 characters");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required")
            .MustAsync(CategoryExists).WithMessage("Category does not exist");
    }

    private async Task<bool> CategoryExists(int categoryId, CancellationToken cancellationToken)
    {
        var allCategories = await _categoryRepository.GetAllCategoriesAsync();
        return allCategories.Any(c => c.Id == categoryId);
    }
}
