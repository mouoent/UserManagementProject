using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace UserManagementProject.Infrastructure.Persistence;

public class UserManagementDbContextFactory : IDesignTimeDbContextFactory<UserManagementDbContext>
{
    public UserManagementDbContext CreateDbContext(string[] args)
    {
        // Create our own host to configure dbcontext in design time 
        var host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<UserManagementDbContext>(options =>
                options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsHistoryTable("_efmigrationshistory", "public"))); 
            })
            .Build();

        var scope = host.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<UserManagementDbContext>();
    }
}
