using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using UserManagementProject.Application.Interfaces;
using UserManagementProject.Infrastructure.Persistence;
using UserManagementProject.Infrastructure.Repositories;
using UserManagementProject.Infrastructure.Services;

namespace UserManagementProject.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Use context at runtime for API availability using DI
            services.AddDbContext<UserManagementDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsHistoryTable("_efmigrationshistory", "public"))); // Making migrations table lowercase for PostgreSQL (my god this was annoying)            

            // Add in-memory cache
            services.AddMemoryCache();

            // Add AuditLogService
            services.AddScoped<IAuditLogService, AuditLogService>();

            // Register repository services
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            return services;
        }
    }
}
