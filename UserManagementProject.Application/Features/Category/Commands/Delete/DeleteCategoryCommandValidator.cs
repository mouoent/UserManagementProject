using FluentValidation;
using UserManagementProject.Application.Interfaces;

namespace UserManagementProject.Application.Features.Category.Commands.Delete;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryCommandValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Must enter a valid Category ID")
            .MustAsync(CategoryExists).WithMessage("Category does not exist.")
            .MustAsync(HaveNoBooks).WithMessage("Category is used for one or more books, delete or update these books and try again.");
    }

    private async Task<bool> CategoryExists(int categoryId, CancellationToken cancellationToken)
    {
        var result = await _categoryRepository.CategoryExistsAsync(categoryId);

        return result;
    }

    // Check if there are any books with the category to be deleted
    private async Task<bool> HaveNoBooks(int categoryId, CancellationToken cancellationToken)
    {
        var result = await _categoryRepository.GetCategoryDetailsByIdAsync(categoryId);

        return !result.BookIds.Any();
    }

}
