using System.ComponentModel.DataAnnotations;

namespace UserManagementProject.Domain.Entities;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
}
