using System.ComponentModel.DataAnnotations;

namespace UserManagementProject.Domain.Entities;

public class Book : BaseEntity
{    
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } 
}
