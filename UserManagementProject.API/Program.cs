using Microsoft.OpenApi.Models;
using UserManagementProject.Application;
using UserManagementProject.Application.Interfaces;
using UserManagementProject.Infrastructure;
using UserManagementProject.Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

var swaggerSettings = builder.Configuration.GetSection("Swagger");
string apiTitle = swaggerSettings.GetValue<string>("Title") ?? "API";
string apiVersion = swaggerSettings.GetValue<string>("Version") ?? "v1";

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(apiVersion, new OpenApiInfo 
    { 
        Title = apiTitle, 
        Version = apiVersion,
        Description = "API for managing users",                 
    });
});

var app = builder.Build();

// Add custom error handler
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Add service for audit table cleanup
using (var scope = app.Services.CreateScope())
{
    var auditLogService = scope.ServiceProvider.GetRequiredService<IAuditLogService>();
    await auditLogService.CleanupOldLogsAsync();
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Management API v1");
    });
}

app.UseAuthorization();
app.MapControllers();

app.Run();