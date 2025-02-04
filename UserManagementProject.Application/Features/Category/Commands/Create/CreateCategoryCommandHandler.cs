using FluentValidation;
using MediatR;
using UserManagementProject.Application.Interfaces;

namespace UserManagementProject.Application.Features.Category.Commands.Create;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
{
    private ICategoryRepository _categoryRepository;
    private IValidator<CreateCategoryCommand> _validator;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IValidator<CreateCategoryCommand> validator)
    {
        _categoryRepository = categoryRepository;
        _validator = validator;
    }

    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        // Validate command
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var category = new Domain.Entities.Category
        {
            Name = request.Name,
        };

        var result = await _categoryRepository.CreateCategoryAsync(category);
        return result;
    }
}
