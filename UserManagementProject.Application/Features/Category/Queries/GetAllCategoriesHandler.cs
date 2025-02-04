using MediatR;
using UserManagementProject.Application.Features.Category.DTOs;
using UserManagementProject.Application.Interfaces;

namespace UserManagementProject.Application.Features.Category.Queries;

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
{
    private ICategoryRepository _categoryRepository;

    public GetAllCategoriesHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await _categoryRepository.GetAllCategoriesAsync();

        return result;
    }
}
