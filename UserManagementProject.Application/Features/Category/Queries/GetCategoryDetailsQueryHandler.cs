using MediatR;
using UserManagementProject.Application.Features.Category.DTOs;
using UserManagementProject.Application.Interfaces;

namespace UserManagementProject.Application.Features.Category.Queries;

public class GetCategoryDetailsQueryHandler : IRequestHandler<GetCategoryDetailsQuery, CategoryDetailsDto>
{
    private ICategoryRepository _categoryRepository;

    public GetCategoryDetailsQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryDetailsDto> Handle(GetCategoryDetailsQuery request, CancellationToken cancellationToken)
    {
        var result = await _categoryRepository.GetCategoryDetailsByIdAsync(request.Id);

        if (result is null)
            throw new KeyNotFoundException("Category not found");

        return result;
    }
}
