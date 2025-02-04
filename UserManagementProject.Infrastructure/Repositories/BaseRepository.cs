using UserManagementProject.Application.Interfaces;
using UserManagementProject.Infrastructure.Persistence;
using System.Text.Json;
using System.Text.Json.Serialization;
using UserManagementProject.Domain.Entities;
using UserManagementProject.Domain.Enums;

namespace UserManagementProject.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly UserManagementDbContext _context;
    protected readonly IAuditLogService _auditLogService;
    protected readonly string EntityIdentifier;

    protected BaseRepository(UserManagementDbContext context, IAuditLogService auditLogService)
    {
        _context = context;
        _auditLogService = auditLogService;
        EntityIdentifier = typeof(TEntity).Name;
    }

    public async Task<TEntity?> GetEntityByIdAsync(int id)
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        if (entity is null) return null;

        // Handle recursive entity relationships
        DetachEntity(entity);

        return entity;
    }

    protected async Task<int> CreateAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();

        // Log creation action
        await _auditLogService.LogChangeAsync(EntityIdentifier, DbActionEnum.Create, entity.ToString(), entity);

        // Return entity's ID (assumes it has an "Id" property)
        return entity.Id;        
    }

    protected async Task UpdateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync();

        // Log update action
        await _auditLogService.LogChangeAsync(EntityIdentifier, DbActionEnum.Update, entity.ToString(), entity);
    }

    protected async Task DeleteAsync(int entityId)
    {
        var entityToDelete = await GetEntityByIdAsync(entityId);

        _context.Set<TEntity>().Remove(entityToDelete);
        await _context.SaveChangesAsync();

        // Log delete action
        await _auditLogService.LogChangeAsync(EntityIdentifier, DbActionEnum.Delete, entityToDelete.ToString(), entityToDelete);
    }

    private void DetachEntity(TEntity entity)
    {
        // Prevents navigation properties from creating cyclic loops
        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles, // Handle cycles
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        _ = JsonSerializer.Serialize(entity, options); // Force serialization to resolve loops
    }
}
