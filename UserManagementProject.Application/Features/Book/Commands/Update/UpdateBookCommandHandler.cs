using FluentValidation;
using MediatR;
using UserManagementProject.Application.Features.Book.Commands.Update;
using UserManagementProject.Application.Interfaces;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Unit>
{
    private readonly IBookRepository _bookRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidator<UpdateBookCommand> _validator;

    public UpdateBookCommandHandler(IBookRepository bookRepository, ICategoryRepository categoryRepository, IValidator<UpdateBookCommand> validator)
    {
        _bookRepository = bookRepository;
        _categoryRepository = categoryRepository;
        _validator = validator;
    }

    public async Task<Unit> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        // Validate command
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        // Fetch book
        var book = await _bookRepository.GetEntityByIdAsync(request.Id);
        if (book is null)
            throw new KeyNotFoundException("Book not found");

        // Fetch category (only if it's being updated)
        if (request.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetEntityByIdAsync(request.CategoryId.Value);
            if (category == null)
                throw new KeyNotFoundException("Category not found");

            book.Category = category;
        }

        // Apply only updated values
        if (!string.IsNullOrWhiteSpace(request.Name))
            book.Name = request.Name;

        // Save changes
        await _bookRepository.UpdateBookAsync(book);

        return Unit.Value;
    }
}
