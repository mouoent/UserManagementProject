using UserManagementProject.Application.Features.Category.DTOs;
using UserManagementProject.Domain.Entities;
using UserManagementProject.Domain.Enums;

namespace UserManagementProject.Application.Interfaces;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<List<CategoryDto>> GetAllCategoriesAsync();
    Task<CategoryDto?> GetCategoryByIdAsync(int categoryId);
    Task<CategoryDetailsDto?> GetCategoryDetailsByIdAsync(int categoryId);
    Task<bool> CategoryExistsAsync(string categoryName);
    Task<bool> CategoryExistsAsync(int categoryId);
    Task<int> CreateCategoryAsync(Category category);
    Task DeleteCategoryAsync(int categoryId);
}
