using UserManagementProject.Domain.Entities;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetEntityByIdAsync(int id);    
}
