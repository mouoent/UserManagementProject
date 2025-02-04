namespace UserManagementProject.Domain.Entities;

public class User : BaseEntity
{    
    public string Name { get; set; }
    public string Surname { get; set; }
    public ICollection<Role> Roles { get; set; } = new List<Role>();
}
