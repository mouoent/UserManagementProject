using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.InteropServices;
using UserManagementProject.Application.Features.Category.DTOs;
using UserManagementProject.Application.Interfaces;
using UserManagementProject.Domain.Entities;
using UserManagementProject.Domain.Enums;
using UserManagementProject.Infrastructure.Persistence;
using UserManagementProject.Infrastructure.Services;

namespace UserManagementProject.Infrastructure.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{    
    private readonly IMemoryCache _cache;

    public CategoryRepository(UserManagementDbContext dbContext, 
        IMemoryCache cache, 
        IAuditLogService auditLogService) 
        : base(dbContext, auditLogService)   
    { }        

    public async Task<List<CategoryDto>> GetAllCategoriesAsync()
    {
        return await _cache.GetOrCreateAsync(EntityIdentifier, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
            return await _context.Categories
                .AsNoTracking()
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        })!;
    }

    public async Task<bool> CategoryExistsAsync(int categoryId)
    {
        // Try fetching from cache
        var cachedCategories = await GetAllCategoriesAsync();
        var cachedCategory = cachedCategories.FirstOrDefault(c => c.Id == categoryId);
        if (cachedCategories.Any())
            return true;

        // If not found in cache, fetch from database 
        return _context.Categories.Any(r => r.Id == categoryId);
    }

    public async Task<bool> CategoryExistsAsync(string categoryName)
    {
        // Try fetching from cache
        var cachedCategories = await GetAllCategoriesAsync();
        var cachedCategory = cachedCategories.FirstOrDefault(c => c.Name == categoryName);
        if (cachedCategories.Any())
            return true;

        // If not found in cache, fetch from database 
        return _context.Categories.Any(r => r.Name == categoryName);
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int categoryId)
    {
        var cachedCategories = await GetAllCategoriesAsync();
        var cachedCategory = cachedCategories.FirstOrDefault(c => c.Id == categoryId);
        if (cachedCategory != null)
            return cachedCategory;

        var result = await _context.Categories
            .Where(c => c.Id == categoryId)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<CategoryDetailsDto?> GetCategoryDetailsByIdAsync(int categoryId)
    {
        var result = await _context.Categories
        .Where(c => c.Id == categoryId)
        .Select(c => new CategoryDetailsDto
        {
            Id = c.Id,
            Name = c.Name,
            BookIds = c.Books.Select(b => b.Id).ToList() // Only fetching book names
        })
        .FirstOrDefaultAsync();

        return result;
    }

    public async Task<int> CreateCategoryAsync(Category category)
    {
        await CreateAsync(category);        
        await RefreshCache(); // Update cache after insertion        

        return category.Id;
    }

    public async Task DeleteCategoryAsync(int categoryId)
    {
        await DeleteAsync(categoryId);
        await RefreshCache();        
    }

    // Refreshes the category cache after insertions or deletion
    private async Task RefreshCache()
    {
        var updatedCategories = await _context.Categories
            .AsNoTracking()
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();

        _cache.Set(EntityIdentifier, updatedCategories, TimeSpan.FromHours(1));
    }
}
