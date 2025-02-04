using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using UserManagementProject.Application.Interfaces;
using UserManagementProject.Domain.Entities;
using UserManagementProject.Infrastructure.Persistence;

namespace UserManagementProject.Infrastructure.Repositories;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    private readonly IMemoryCache _cache;    

    public RoleRepository(UserManagementDbContext dbContext, 
        IAuditLogService auditLogService,
        IMemoryCache cache) 
        : base(dbContext, auditLogService)
    { }

    public async Task<List<Role>> GetAllRolesAsync()
    {
        return await _cache.GetOrCreateAsync(EntityIdentifier, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
            return await _context.Roles.AsNoTracking().ToListAsync(); // Prevent EF tracking issues
        });
    }

    public async Task<bool> RoleExistsAsync(int roleId)
    {
        // Try fetching from cache
        var cachedRoles = await GetAllRolesAsync();
        var cachedRole = cachedRoles.FirstOrDefault(c => c.Id == roleId);
        if (cachedRoles.Any())
            return true;

        // If not found in cache, fetch from database 
        return _context.Roles.Any(r => r.Id == roleId);
    }

    public async Task<List<Role>> GetRolesByIdsAsync(List<int> roleIds)
    {
        var allRoles = await GetAllRolesAsync();
        var filteredRoles = allRoles.Where(r => roleIds.Contains(r.Id)).ToList();

        // Reattach roles to the context to avoid detached entity issues
        foreach (var role in filteredRoles)
        {
            _context.Roles.Attach(role);
        }

        return filteredRoles;
    }    
}
