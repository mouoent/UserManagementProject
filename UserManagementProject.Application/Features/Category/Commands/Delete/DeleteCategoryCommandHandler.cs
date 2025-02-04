using FluentValidation;
using MediatR;
using UserManagementProject.Application.Interfaces;

namespace UserManagementProject.Application.Features.Category.Commands.Delete;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Unit>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidator<DeleteCategoryCommand> _validator;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IValidator<DeleteCategoryCommand> validator)
    {
        _categoryRepository = categoryRepository;
        _validator = validator;
    }

    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        await _categoryRepository.DeleteCategoryAsync(request.Id);

        return Unit.Value;
    }
}
