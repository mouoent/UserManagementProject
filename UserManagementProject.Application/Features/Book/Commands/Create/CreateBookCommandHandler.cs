using MediatR;
using FluentValidation;
using UserManagementProject.Application.Interfaces;

namespace UserManagementProject.Application.Features.Book.Commands.Create;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, int>
{
    private readonly IBookRepository _bookRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidator<CreateBookCommand> _validator;

    public CreateBookCommandHandler(IBookRepository bookRepository, ICategoryRepository categoryRepository, IValidator<CreateBookCommand> validator)
    {
        _bookRepository = bookRepository;
        _categoryRepository = categoryRepository;
        _validator = validator;
    }

    public async Task<int> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        // Validate command
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        // Fetch category from the database (avoid duplicate queries)
        var category = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId);
        if (category is null)
            throw new KeyNotFoundException("Category not found");

        var book = new Domain.Entities.Book
        {
            Name = request.Name,
            CategoryId = category.Id
        };

        var result = await _bookRepository.CreateBookAsync(book);
        return result;
    }
}
