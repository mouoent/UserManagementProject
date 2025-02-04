using UserManagementProject.Domain.Enums;

namespace UserManagementProject.Application.Interfaces;

public interface IAuditLogService
{
    Task LogChangeAsync(string tableName, DbActionEnum action, string entityId, object changes);
    Task CleanupOldLogsAsync();
}
