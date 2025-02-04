using UserManagementProject.Application.Features.User.DTOs;
using UserManagementProject.Domain.Entities;

namespace UserManagementProject.Application.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<List<UserDto>> GetAllUsersAsync();
    Task<UserDetailsDto> GetUserDetailsByIdAsync(int userId);
    Task<int> CreateUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task<bool> UserExistsAsync(int userId);    
}
