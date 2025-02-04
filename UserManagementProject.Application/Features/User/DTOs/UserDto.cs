namespace UserManagementProject.Application.Features.User.DTOs;

public record UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
}
