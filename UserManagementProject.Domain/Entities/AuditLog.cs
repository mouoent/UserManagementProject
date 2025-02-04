using UserManagementProject.Domain.Enums;

namespace UserManagementProject.Domain.Entities;

public class AuditLog : BaseEntity
{
    public string TableName { get; set; } = string.Empty; // Table being modified
    public DbActionEnum Action { get; set; } 
    public string EntityId { get; set; } = string.Empty; // ID of the entity modified
    public string Changes { get; set; } = string.Empty; // JSON serialized changes
    public DateTime Timestamp { get; set; } = DateTime.UtcNow; // Time of change
}
