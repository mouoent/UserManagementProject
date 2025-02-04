using Microsoft.EntityFrameworkCore;
using UserManagementProject.Application.Features.User.DTOs;
using UserManagementProject.Application.Interfaces;
using UserManagementProject.Domain.Entities;
using UserManagementProject.Domain.Enums;
using UserManagementProject.Infrastructure.Persistence;

namespace UserManagementProject.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{    
    public UserRepository(UserManagementDbContext context, IAuditLogService auditLogService) : base (context, auditLogService)
    {}

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var result = await _context.Users
                .AsNoTracking()
                .Select(c => new UserDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Surname = c.Surname,
                })
                .ToListAsync();

        return result;
    }

    public async Task<UserDetailsDto> GetUserDetailsByIdAsync(int userId)
    {
        var result = await _context.Users
            .Where(c => c.Id == userId)
            .Select(c => new UserDetailsDto
            {
                Id = c.Id,
                Name = c.Name,
                Surname = c.Surname,
                RoleIds = c.Roles.Select(b => (int)b.Id).ToList() // Only fetching role ids
            })
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<bool> UserExistsAsync(int userId)
    {
        return await _context.Users.AnyAsync(u => u.Id == userId);
    }

    public async Task<int> CreateUserAsync(User user)
    {
        await CreateAsync(user);

        return user.Id;
    }

    public async Task UpdateUserAsync(User user)
    {
        await UpdateAsync(user);
    }    
}
