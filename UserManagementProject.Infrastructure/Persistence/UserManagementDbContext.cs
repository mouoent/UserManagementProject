using Microsoft.EntityFrameworkCore;
using UserManagementProject.Domain.Entities;

namespace UserManagementProject.Infrastructure.Persistence;

public class UserManagementDbContext : DbContext
{    
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }    
    public DbSet<Category> Categories { get; set; }
    public DbSet<Book> Books { get; set; }    
    public DbSet<AuditLog> AuditLogs { get; set; }

    public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options)
        : base(options)
    {        
    }    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {            
            throw new InvalidOperationException("Use IDesignTimeDbContextFactory or configure options externally.");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {        
        // Force lowercase table names for PostgreSQL compatibility
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.GetTableName().ToLower());
        }       
        
        // One to many: Books -> Categories
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Category)
            .WithMany()
            .HasForeignKey(b => b.CategoryId);

        // Create Category table
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Books)
            .WithOne(b => b.Category)
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict); // All books with a category must be deleted first       

        // Create Role table
        modelBuilder.Entity<Role>()
            .Property(r => r.Id)
            .HasConversion<int>();

        // Seed Role table 
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "User" },
            new Role { Id = 3, Name = "Moderator" }
        );

        // Seed default Users
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "Tony", Surname = "Karounis" },
            new User { Id = 2, Name = "Donald", Surname = "Trump" },
            new User { Id = 3, Name = "Joe", Surname = "Biden" }
        );

        // Many to many: Users <-> Roles (creates and seeds UserRoles table)
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "userroles",
                j => j.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),
                j =>
                {
                    j.HasKey("UserId", "RoleId");
            
                    j.HasData(
                        new { UserId = 1, RoleId = 1 }, // Admin
                        new { UserId = 2, RoleId = 2 }, // User
                        new { UserId = 3, RoleId = 3 }  // Moderator
                    );
                }
            );

        // Seed Books table
        modelBuilder.Entity<Book>().HasData(
            new Book { Id = 1, Name = "The Hobbit", CategoryId = 1 },
            new Book { Id = 2, Name = "Dune", CategoryId = 2 },
            new Book { Id = 3, Name = "Sherlock Holmes", CategoryId = 3 },
            new Book { Id = 4, Name = "Steve Jobs", CategoryId = 4 },
            new Book { Id = 5, Name = "World War II", CategoryId = 5 }
        );

        // Seed Categories table
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Fantasy" },
            new Category { Id = 2, Name = "Science Fiction" },
            new Category { Id = 3, Name = "Mystery" },
            new Category { Id = 4, Name = "Biography" },
            new Category { Id = 5, Name = "History" }
        );       

    }    
}
