using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using UserManagementProject.Application.Interfaces;
using UserManagementProject.Domain.Entities;
using UserManagementProject.Domain.Enums;
using UserManagementProject.Infrastructure.Persistence;

namespace UserManagementProject.Infrastructure.Services;

public class AuditLogService : IAuditLogService
{
    private readonly UserManagementDbContext _context;

    public AuditLogService(UserManagementDbContext context)
    {
        _context = context;
    }

    public async Task LogChangeAsync(string tableName, DbActionEnum action, string entityId, object changes)
    {
        var log = new AuditLog
        {
            TableName = tableName,
            Action = action,
            EntityId = entityId,
            Changes = JsonConvert.SerializeObject(changes),
            Timestamp = DateTime.UtcNow
        };

        _context.AuditLogs.Add(log);
        await _context.SaveChangesAsync();
    }

    public async Task CleanupOldLogsAsync()
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-20);
        _context.AuditLogs.Where(log => log.Timestamp < cutoffDate).ExecuteDelete();
    }
}
