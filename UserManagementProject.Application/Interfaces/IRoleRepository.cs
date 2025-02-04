using UserManagementProject.Domain.Entities;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<List<Role>> GetAllRolesAsync();
    Task<bool> RoleExistsAsync(int roleId);
    Task<List<Role>> GetRolesByIdsAsync(List<int> roleIds);

}
